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

    // Variables de salto
    [SerializeField] protected LayerMask groundLayer;
    public bool onGround = false;

    // Variables de combate
    protected bool atacando = false;
    protected bool isDashing = false;
    [SerializeField] List<GameObject> hitsAtaques;
    [SerializeField] protected float attackDamage;
    public bool daniado = false;
    public bool isKnockback = false;

    // Start is called before the first frame update
    protected void Start()
    {
        foreach (GameObject hit in hitsAtaques)
        {
            hit.GetComponent<AttackHitbox>().SetDamage(attackDamage);
        }
        rb2D = GetComponent<Rigidbody2D>();
        animator = gameObject.GetComponent<Animator>();
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
        rb2D.AddForce(Vector2.up*jumpForce, ForceMode2D.Impulse);
        animator.SetTrigger("Jump");            
        StartCoroutine(Aterrizaje());
    }

    public void Dash()
    {
        rb2D.AddForce(Vector2.right*transform.localScale.x*dashForce, ForceMode2D.Impulse);
        isDashing = true;
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
    
    protected abstract void Atacar(int id);
}

