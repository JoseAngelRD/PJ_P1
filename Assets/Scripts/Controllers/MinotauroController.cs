using System;
using UnityEngine;
using UnityEngine.UIElements;

public class MinotauroController : CharacterController
{
    [Header("Configuración IA")]
    public GameObject player;
    //public float rangoDeAtaque = 2f;
    //public float distanciaEsquive = 4f;
    public float cooldownMaximo = 2f;
    private int direccionHuida = 0;
    // Referencia al script que maneja la vida    

    private NodoArbol raizArbol;

    // Usamos 'new' y 'base.Start()' para no perder la inicialización de hitboxes, rb2D y animator del padre
    protected new void Start()
    {
        base.Start();
        ConstruirArbolIA();
    }

    private void ConstruirArbolIA()
    {
        // --- 1. ACCIONES ---
        Accion pego = new AtaqueBasico(this);
        Accion esquivo = new Esquivo(this);
        Accion meAcerco = new MeAcerco(this);
        Accion meQuedoQuieto = new MeQuedoQuieto(this);
        Accion seguirUltimaAccion = new SeguirUltimaAccion();

        // --- 2. NODOS DE PROBABILIDAD ---
        Decision probCombateSi50 = new ProbabilidadDecision(pego, esquivo, 60f);
        Decision probCombateNo50 = new ProbabilidadDecision(pego, esquivo, 40f);

        Decision probMovSi50 = new ProbabilidadDecision(meQuedoQuieto, meAcerco, 5f);
        Decision probMovNo50 = new ProbabilidadDecision(meQuedoQuieto, meAcerco, 30f);

        // --- 3. RAMA IZQUIERDA (EstaEnRango? -> Si) ---
        Decision coolCombateSi50 = new EstoyCooldown(seguirUltimaAccion, probCombateSi50, this);
        Decision coolCombateNo50 = new EstoyCooldown(seguirUltimaAccion, probCombateNo50, this);

        Decision menos50Ataca = new MenosMitadVida(coolCombateSi50, coolCombateNo50, this);
        Decision coolNoAtaca = new EstoyCooldown(seguirUltimaAccion, pego, this);

        Decision meAtaca = new MeAtaca(menos50Ataca, coolNoAtaca);

        // --- 4. RAMA DERECHA (EstaEnRango? -> No) ---
        Decision coolMovSi50 = new EstoyCooldown(seguirUltimaAccion, probMovSi50, this);
        Decision coolMovNo50 = new EstoyCooldown(seguirUltimaAccion, probMovNo50, this);

        Decision menos50Lejos = new MenosMitadVida(coolMovSi50, coolMovNo50, this);

        // --- 5. RAÍZ ---
        raizArbol = new EnRangoMelee(meAtaca, menos50Lejos, this.gameObject);
    }

    void Update()
    {        
        // Acciones que paran la IA
        if (vidaActual <= 0 || daniado || isKnockback || GameManager.gameM.isGameOver) {            
            //Debug.Log("Vida Actual: " + vidaActual + ", Dañado: " + daniado + ", Knockback: " + isKnockback);
            return;
        }

        if (cooldownActual > 0) cooldownActual -= Time.deltaTime;

        if (atacando || isDashing)
        {            
            movimiento = Vector2.zero;
            return;
        }

        // Ejecutar árbol de decisiones de la IA
        if (raizArbol != null && player != null)
        {
            NodoArbol nodoFinal = raizArbol.Decide(player);
            if (nodoFinal is Accion accion)
            {
                accion.EjecutarAccion(player);
            }
        }

        // Volteo de sprite (Flip)
        if (movimiento.x < 0.0f) transform.localScale = new Vector3(Math.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
        else if (movimiento.x > 0.0f) transform.localScale = new Vector3(-Math.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);

        // Actualizamos el Animator (ya instanciado en CharacterController)
        animator.SetFloat("Horizontal", movimiento.x);
        animator.SetFloat("Speed", movimiento.magnitude);
    }

    void FixedUpdate()
    {
        // Respetamos fisicas externas
        if (daniado || isKnockback) return;
        
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("Death"))
        {            
            if (direccionHuida == 0)
            {
                direccionHuida = Math.Sign(transform.position.x-player.transform.position.x);
                Debug.Log(direccionHuida);
                if (direccionHuida == 0)
                {
                    direccionHuida = 1;
                }
            }

            // Desactivar el box fisico para que huya fuera del mapa
            foreach (BoxCollider2D box in GetComponentsInChildren<BoxCollider2D>())
            {
                if (!box.isTrigger)
                {
                    box.enabled = false;
                    break;
                }
            }

            transform.localScale = new Vector3(Math.Abs(transform.localScale.x)*-direccionHuida, transform.localScale.y, transform.localScale.z);
            rb2D.velocity = new Vector2(direccionHuida*10, 0);
        } else
        {
            rb2D.velocity = new Vector2(movimiento.x * velocidad, rb2D.velocity.y);
        }
    }
    public override void Atacar(int id)
    {
        if (id == 0)
        {
            animator.SetTrigger("Attack");
            atacando = true;
            cooldownActual = cooldownMaximo;
        }
    }

    public void FinalizarCombate()
    {
        GameManager.gameM.isGameOver = true;        
        // Buscamos el cronometro y le pasamos el nombre del Boss
        Cronometro crono = FindObjectOfType<Cronometro>();
        if (crono != null)
        {
            //Debug.Log("Minotauro derrotado, deteniendo cronómetro...");
            crono.DetenerYComprobarRecord("Minotauro");
            //Debug.Log("detenerYComprobarRecord ejecutado");
        }
        menuMuerte.SetActive(true);
    }
}

