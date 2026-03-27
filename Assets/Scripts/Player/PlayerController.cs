using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float velocidad = 5.0f;
    [SerializeField] private float jumpForce = 150f;
    private Rigidbody2D rb2D;
    private Vector2 movimiento;
    private Animator animator;

    // Variables de salto
    [SerializeField] private LayerMask groundLayer;
    public bool onGround = false;

    // Variables de combate
    private bool atacando = false;    
    [SerializeField] List<GameObject> hitsAtaques;
    [SerializeField] private float attackDamage;
    public bool daniado = false;
    public bool isKnockback = false;

    // Start is called before the first frame update
    void Start()
    {
        /*foreach (GameObject hit in hitsAtaques)
        {
            hit.GetComponent<AttackHitbox>().SetDamage(attackDamage);
        }*/
        rb2D = GetComponent<Rigidbody2D>();
        animator = gameObject.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        // Estado del player
        Debug.DrawRay(transform.position, Vector3.down * 0.4f, Color.red);
        RaycastHit2D colisionDown = Physics2D.Raycast(transform.position, Vector3.down, 0.4f, groundLayer);
        if (colisionDown != false && colisionDown.collider.CompareTag("Suelo")) onGround = true;
        else onGround = false;

        /*if (daniado)
        {
            movimiento = Vector2.zero;
            return;
        }*/
        if (atacando)
        {            
            if (onGround) movimiento = Vector2.zero;
            if (animator.GetCurrentAnimatorStateInfo(0).IsName("Idle"))
            {
                Debug.Log("Cancelado el ataque");
                EndAttack();
            }         
            return;
        }
        
        movimiento.x = Input.GetAxisRaw("Horizontal");
        movimiento = movimiento.normalized;

        if (movimiento.x < 0.0f) transform.localScale = new Vector3 (-Math.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
        else if (movimiento.x > 0.0f) transform.localScale = new Vector3 (Math.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);        
                
        animator.SetFloat("Horizontal", movimiento.x);        
        animator.SetFloat("Speed", movimiento.magnitude);

        if (Input.GetMouseButtonDown(0)) {            
            Debug.Log("LeftClick");
            animator.SetTrigger("Attack");
            atacando = true;            
        }

        if (Input.GetKey(KeyCode.A))
        {
            animator.SetInteger("Direccion", 0);
        } else if (Input.GetKey(KeyCode.D))
        {
            animator.SetInteger("Direccion", 1);
        }

        if ((Input.GetKey(KeyCode.Space) || Input.GetKey(KeyCode.W)) && onGround && rb2D.velocity.y < 0.1f)
        {            
            rb2D.AddForce(Vector2.up*jumpForce, ForceMode2D.Impulse);
            animator.SetTrigger("Jump");            
            StartCoroutine(Aterrizaje());            
        }
    }

    void FixedUpdate()
    {        
        if (isKnockback)
        {
            return;
        }
        rb2D.velocity = new Vector2(movimiento.x * velocidad, rb2D.velocity.y);
    }  

    private IEnumerator Aterrizaje()
    {
        // Espera a salir del suelo
        yield return new WaitUntil(() => !onGround);
        //Debug.Log("Salgo del suelo");
        
        // Esperar a que aterrize
        yield return new WaitUntil(() => onGround && Mathf.Abs(rb2D.velocity.y) < 0.01f);
        //Debug.Log("Aterrizo");
        animator.SetTrigger("OnGround");
    }

    public void ActivarHit(int index)
    {
        hitsAtaques[index].SetActive(true);
    }

    public void EndAttack()
    {
        foreach (GameObject h in hitsAtaques)
        {
            h.SetActive(false);
        }        
        atacando = false;        
    }

    public void EndHurt()
    {
        daniado = false;
    }
}

