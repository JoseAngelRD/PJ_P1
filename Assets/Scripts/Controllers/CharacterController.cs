using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CharacterController : MonoBehaviour
{
    [SerializeField] protected float velocidad = 5.0f;
    [SerializeField] protected float jumpForce = 150f;
    [SerializeField] public float dashForce = 10f;
    [SerializeField] public float dashDuration = 0.2f;
    [SerializeField] protected float tamRaycast = 0.4f;
    protected Rigidbody2D rb2D;
    public Vector2 movimiento;
    protected Animator animator;

    [HideInInspector] public float cooldownActual = 0f;
    // Variables de salto
    [SerializeField] protected LayerMask groundLayer;
    public bool onGround = false;

    // Variables de combate
    public bool accionActiva = false;
    [SerializeField] public bool atacando = false;
    [SerializeField] public bool isDashing = false;
    [SerializeField] List<GameObject> hitsAtaques;
    [SerializeField] protected float attackDamage;
    public bool daniado = false;
    public bool isKnockback = false;
    [SerializeField] private AudioClip ataqueSFX;
    [SerializeField] private AudioClip dashSFX;
    protected DamageReceiver damageReceiver;
    public float vidaActual 
    { 
        get { return damageReceiver != null ? damageReceiver.GetVida() : 0f; } 
    }
    
    public float vidaMaxima 
    { 
        get { return damageReceiver != null ? damageReceiver.GetVidaMaxima() : 100f; } 
    }

    [SerializeField] protected GameObject menuMuerte = null;

    // Start is called before the first frame update
    protected void Start()
    {
        foreach (GameObject hit in hitsAtaques)
        {
            hit.GetComponent<AttackHitbox>().SetDamage(attackDamage);
        }
        rb2D = GetComponent<Rigidbody2D>();        
        damageReceiver = GetComponentInChildren<DamageReceiver>();
        animator = gameObject.GetComponent<Animator>();
        damageReceiver = GetComponentInChildren<DamageReceiver>();
        if (damageReceiver == null) damageReceiver = GetComponent<DamageReceiver>();
    }

    protected IEnumerator Aterrizaje()
    {
        // Espera a salir del suelo
        yield return new WaitUntil(() => !onGround);
        //Debug.Log("Salgo del suelo");

        // Esperar a que aterrize
        yield return new WaitUntil(() => onGround && Mathf.Abs(rb2D.velocity.y) < 0.01f);
        //Debug.Log("Aterrizo");
        animator.SetTrigger("OnGround");
    }

    protected void Saltar()
    {
        rb2D.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        animator.SetTrigger("Jump");
        StartCoroutine(Aterrizaje());
    }

    public void HacerDash()
    {
        if (isDashing) return;
        isDashing = true;
        Debug.Log("Dash desde objeto: " + gameObject.name +
              " | instanceID: " + GetInstanceID() +
              " | frame: " + Time.frameCount);      
        StartCoroutine(Dash());
    }

    public IEnumerator Dash()
    {                       
        GameManager.gameM.ReproducirSonido(dashSFX, 0);             
        if (rb2D != null)
        {
            rb2D.velocity = Vector2.right * transform.localScale.x * dashForce;
        }           
        //rb2D.AddForce(Vector2.right * transform.localScale.x * dashForce, ForceMode2D.Impulse);        

        yield return new WaitForSeconds(dashDuration);
        //rb2D.velocity = Vector2.zero;
        isDashing = false;
    }

    public void ActivarHit(int index)
    {
        hitsAtaques[index].GetComponent<BoxCollider2D>().enabled = true;
    }

    public void EndHit()
    {
        foreach (GameObject h in hitsAtaques)
        {
            h.GetComponent<BoxCollider2D>().enabled = false;
        }
    }

    public void EndAttack()
    {
        foreach (GameObject h in hitsAtaques)
        {
            h.GetComponent<BoxCollider2D>().enabled = false;
        }
        atacando = false;
        isDashing = false;
    }

    public void EndHurt()
    {
        daniado = false;
    }

    private void ActivarMenuMuerte()
    {
        GameManager.gameM.isGameOver = true;
        if (menuMuerte != null)
        {
            menuMuerte.SetActive(true);
        }
    }

    public void SonidoAtaque(float pitchMin)
    {
        GameManager.gameM.ReproducirSonido(ataqueSFX, pitchMin);
    }

    public abstract void Atacar(int id);

    public void SetMovimiento(Vector2 nuevoMovimiento)
    {
        movimiento = nuevoMovimiento;
    }

    public void Apuntar(GameObject player, bool aObjetivo)
    {        
        float direccionX = player.transform.position.x - transform.position.x;

        float signo;
        if (gameObject.name == "Samurai")
        {
            signo = aObjetivo ? Mathf.Sign(direccionX) : -Mathf.Sign(direccionX);
        } else
        {
            signo = aObjetivo ? -Mathf.Sign(direccionX) : Mathf.Sign(direccionX);
        }
        transform.localScale = new Vector3(signo * Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
    }

    public IEnumerator EjecutarConGuardia(IEnumerator corrutina, float cooldown)
    {
        accionActiva = true;
        //Debug.Log("ACCION ACTIVA");
        yield return StartCoroutine(corrutina);
        //Debug.Log("FIN ACCION");
        yield return new WaitForSeconds(cooldown);
        accionActiva = false;
    }

}