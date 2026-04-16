using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CharacterController : MonoBehaviour
{
    [SerializeField] protected float velocidad = 5.0f;
    [SerializeField] protected float jumpForce = 150f;
    [SerializeField] private float dashForce = 10f;
    [SerializeField] protected float tamRaycast = 0.4f;
    protected Rigidbody2D rb2D;
    public Vector2 movimiento;
    protected Animator animator;

    [HideInInspector] public float cooldownActual = 0f;
    // Variables de salto
    [SerializeField] protected LayerMask groundLayer;
    public bool onGround = false;

    // Variables de combate
    [SerializeField] public bool atacando = false;
    protected bool isDashing = false;
    [SerializeField] List<GameObject> hitsAtaques;
    [SerializeField] protected float attackDamage;
    public bool daniado = false;
    public bool isKnockback = false;
    [SerializeField] protected bool shield = false;
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

    public IEnumerator Dash()
    {
        GameManager.gameM.ReproducirSonido(dashSFX, 0);
        rb2D.AddForce(Vector2.right * transform.localScale.x * dashForce, ForceMode2D.Impulse);
        isDashing = true;

        yield return new WaitForSeconds(0.2f);
        rb2D.velocity = Vector2.zero;
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

}