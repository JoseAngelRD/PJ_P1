using System;
using UnityEngine;

public class SamuraiController : CharacterController
{
    [Header("Configuración IA")]
    public GameObject player;    
    private NodoArbol raizArbol;    
    protected new void Start()
    {
        base.Start();
        ConstruirArbolIA();        
    }

    private void ConstruirArbolIA()
    {
        // ACCIONES
        Accion pego = new AtaqueBasico(this);
        Accion meAlejo = new Esquivo(this);
        Accion meAcerco = new MeAcerco(this); 
        Accion meQuedoQuieto = new MeQuedoQuieto(this);
        Accion dashDefensivo = new DashDefensivo(this);
        Accion dashOfensivo = new DashOfensivo(this);
        Accion dashAtaque = new DashAtaque(this);
        Accion combo = new Combo(this);

        // PROBABILIDADES
        Decision prob_BajoAtaque_VidaBaja_Si = new ProbabilidadDecision(dashDefensivo, meAlejo, 50f); 
        Decision prob_BajoAtaque_VidaBaja_No = new ProbabilidadDecision(meAlejo, meQuedoQuieto, 80f); 
        
        Decision prob_NoAtaca_VidaBaja_Si = new ProbabilidadDecision(pego, meAlejo, 60f);                
        Decision prob_NoAtaca_VidaBaja_No = new ProbabilidadDecision(pego, meQuedoQuieto, 40f);
        
        Decision prob_enDash_VidaBaja_Si = new ProbabilidadDecision(dashAtaque, combo, 50f);
        Decision prob_enDash_VidaBaja_No = new ProbabilidadDecision(dashAtaque, meAcerco, 80f);  

        Decision prob_noEnDash_VidaBaja_Si = new ProbabilidadDecision(dashOfensivo, meAcerco, 50f);
        Decision prob_noEnDash_VidaBaja_No = new ProbabilidadDecision(meAcerco, meQuedoQuieto, 40f);  

        // RAMA DERECHA
        Decision bajoAtaque = new MenosMitadVida(prob_BajoAtaque_VidaBaja_Si, prob_BajoAtaque_VidaBaja_No, this);
        Decision noAtaca = new MenosMitadVida(prob_NoAtaca_VidaBaja_Si, prob_NoAtaca_VidaBaja_No, this);        

        Decision meAtaca = new MeAtaca(bajoAtaque, noAtaca);

        // RAMA IZQUIERDA
        Decision enRangoDash_Si = new MenosMitadVida(prob_enDash_VidaBaja_Si, prob_enDash_VidaBaja_No, this);
        Decision enRangoDash_No = new MenosMitadVida(prob_noEnDash_VidaBaja_Si, prob_noEnDash_VidaBaja_No, this); 

        Decision enRangoDash = new EnRangoDash(enRangoDash_Si, enRangoDash_No, this.gameObject);

        // RAIZ
        raizArbol = new EnRangoMelee(meAtaca, enRangoDash, this.gameObject); 
    }

    void Update()
    {        
        // Acciones que paran la IA
        if (vidaActual <= 0 || daniado || isKnockback || accionActiva || GameManager.gameM.isGameOver) 
        {            
            return;
        }

        if (atacando || isDashing)
        {            
            movimiento = Vector2.zero;
            return;
        }

        // Arbol de decisiones
        if (raizArbol != null && player != null && !accionActiva)
        {
            NodoArbol nodoFinal = raizArbol.Decide(player);
            if (nodoFinal is Accion accion)
            {
                accion.EjecutarAccion(player);                
            }
        }

        // Volteo de sprite (Flip)
        if (movimiento.x < 0.0f) transform.localScale = new Vector3(-Math.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
        else if (movimiento.x > 0.0f) transform.localScale = new Vector3(Math.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);

        // Actualizamos el Animator
        animator.SetFloat("Horizontal", movimiento.x);
        animator.SetFloat("Speed", movimiento.magnitude);
    }

    void FixedUpdate()
    {
        // Respetar las fisicas externas
        if (vidaActual <= 0 || daniado || isKnockback || isDashing) return;         
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
        GameManager.gameM.isGameOver = true;
        // Buscamos el cronometro y le pasamos el nombre del Boss
        Cronometro crono = FindObjectOfType<Cronometro>();
        if (crono != null)
        {
            //Debug.Log("Samurai derrotado, deteniendo cronómetro...");
            crono.DetenerYComprobarRecord("Samurai");
            //Debug.Log("detenerYComprobarRecord ejecutado");
        }
        menuMuerte.SetActive(true);
    }
}