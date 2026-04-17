using System;
using UnityEngine;

public class SamuraiController : CharacterController
{
    [Header("Configuración IA")]
    public GameObject player;    
    private NodoArbol raizArbol;    
    
    // Usamos 'new' y 'base.Start()' para no perder la inicialización de hitboxes, rb2D y animator del padre
    protected new void Start()
    {
        base.Start();
        ConstruirArbolIA();
        //Time.timeScale = 0.1f;
    }

    private void ConstruirArbolIA()
    {
        // --- 1. ACCIONES ---
        Accion pego = new AtaqueBasico(this);
        Accion esquivo = new Esquivo(this);
        Accion meAcerco = new MeAcerco(this); 
        Accion meQuedoQuieto = new MeQuedoQuieto(this);
        Accion dashDefensivo = new DashDefensivo(this);
        Accion dashOfensivo = new DashOfensivo(this);
        Accion dashAtaque = new DashAtaque(this);
        Accion combo = new Combo(this);

        Accion nada = new SeguirUltimaAccion();

        // --- 2. NODOS DE PROBABILIDAD ---
        Decision prob_BajoAtaque_VidaBaja_Si = new ProbabilidadDecision(dashDefensivo, meQuedoQuieto, 20f); 
        Decision prob_BajoAtaque_VidaBaja_No = new ProbabilidadDecision(esquivo, meQuedoQuieto, 80f); 
        
        Decision prob_NoAtaca_VidaBaja_Si = new ProbabilidadDecision(pego, nada, 60f);                
        Decision prob_NoAtaca_VidaBaja_No = new ProbabilidadDecision(pego, nada, 40f);
        
        Decision prob_enDash_VidaBaja_Si = new ProbabilidadDecision(dashAtaque, combo, 50f);
        Decision prob_enDash_VidaBaja_No = new ProbabilidadDecision(dashAtaque, meAcerco, 10f);  

        Decision prob_noEnDash_VidaBaja_Si = new ProbabilidadDecision(dashOfensivo, meAcerco, 50f);
        Decision prob_noEnDash_VidaBaja_No = new ProbabilidadDecision(meAcerco, nada, 40f);  

        // --- 3. RAMA IZQUIERDA (EstaEnRangoMelee? -> Si) ---
        Decision bajoAtaque = new MenosMitadVida(prob_BajoAtaque_VidaBaja_Si, prob_BajoAtaque_VidaBaja_No, this);
        Decision noAtaca = new MenosMitadVida(prob_NoAtaca_VidaBaja_Si, prob_NoAtaca_VidaBaja_No, this);        

        Decision meAtaca = new MeAtaca(bajoAtaque, noAtaca);

        // --- 4. RAMA DERECHA (EstaEnRangoMelee? -> No) ---
        Decision enRangoDash_Si = new MenosMitadVida(prob_enDash_VidaBaja_Si, prob_enDash_VidaBaja_No, this);
        Decision enRangoDash_No = new MenosMitadVida(prob_noEnDash_VidaBaja_Si, prob_noEnDash_VidaBaja_No, this); 

        Decision enRangoDash = new EnRangoDash(enRangoDash_Si, enRangoDash_No, this.gameObject);

        // --- 5. RAÍZ ---
        raizArbol = new EnRangoMelee(meAtaca, enRangoDash, this.gameObject); 
    }

    void Update()
    {        
        if (accionActiva) {            
            return;
        }

        // Si el boss está muerto, dañado o sufriendo knockback, detenemos la IA para respetar físicas y animaciones
        if (vidaActual <= 0 || daniado || isKnockback) 
        {
            //Debug.Log("Vida Actual: " + vidaActual + ", Dañado: " + daniado + ", Knockback: " + isKnockback);
            return;
        }

        if (atacando || isDashing)
        {            
            movimiento = Vector2.zero;
            return;
        }

        // Ejecutar árbol de decisiones de la IA
        Debug.Log("Ejecutando arbol");
        if (raizArbol != null && player != null && !accionActiva)
        {
            NodoArbol nodoFinal = raizArbol.Decide(player);
            if (nodoFinal is Accion accion)
            {
                accion.EjecutarAccion(player);
                Debug.Log(accion);
            }
        }
        /*if (raizArbol != null && player != null)
        {
            NodoArbol nodoFinal = raizArbol.Decide(player);
            if (nodoFinal is Accion accion)
            {
                accion.EjecutarAccion(player);
            }
        }*/

        // Volteo de sprite (Flip)
        if (movimiento.x < 0.0f) transform.localScale = new Vector3(-Math.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
        else if (movimiento.x > 0.0f) transform.localScale = new Vector3(Math.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);

        // Actualizamos el Animator (ya instanciado en CharacterController)
        animator.SetFloat("Horizontal", movimiento.x);
        animator.SetFloat("Speed", movimiento.magnitude);
    }

    void FixedUpdate()
    {
        // Respetamos físicas externas (como el Knockback) si está sufriendo daño
        if (vidaActual <= 0 || daniado || isKnockback) return; 
        if (isDashing)  {            
            return;
        }
        rb2D.velocity = new Vector2(movimiento.x * velocidad, rb2D.velocity.y);
    }    

    public override void Atacar(int id)
    {        
        switch(id)
        {
            case 0:
            {             
                movimiento = Vector2.zero;   
                animator.SetTrigger("Attack1");
                atacando = true; 
            }
            break;
            case 1:
            {             
                movimiento = Vector2.zero;     
                animator.SetTrigger("Attack2");                        
                atacando = true; 
            }
            break;
        }
    }
    
    public void FinalizarCombate()
    {
        // 1. Buscamos el cronómetro y le pasamos el nombre del Boss
        Cronometro crono = FindObjectOfType<Cronometro>();
        if (crono != null)
        {
            Debug.Log("Samurai derrotado, deteniendo cronómetro...");
            crono.DetenerYComprobarRecord("Samurai");
            Debug.Log("detenerYComprobarRecord ejecutado");
        }
        menuMuerte.SetActive(true);
    }
}