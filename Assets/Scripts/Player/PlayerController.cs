using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float velocidad = 5.0f;
    private Rigidbody2D rb2D;
    private Vector2 movimiento;
    private Animator animator;
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
        /*if (daniado)
        {
            movimiento = Vector2.zero;
            return;
        }
        if (atacando)
        {
            movimiento = Vector2.zero;
            hitsAtaques[animator.GetInteger("Direccion")].SetActive(true);
            return;
        }*/

        movimiento.x = Input.GetAxisRaw("Horizontal");
        movimiento = movimiento.normalized;
                
        animator.SetFloat("Horizontal", movimiento.x);
        animator.SetFloat("Vertical", movimiento.y);
        animator.SetFloat("Speed", movimiento.magnitude);

        /*if (Input.GetMouseButtonDown(0)) {            
            Debug.Log("LeftClick");
            animator.SetTrigger("Attack");
            atacando = true;            
        }*/ 

        if (Input.GetKey(KeyCode.A))
        {
            animator.SetInteger("Direccion", 0);
        } else if (Input.GetKey(KeyCode.D))
        {
            animator.SetInteger("Direccion", 1);
        }
    }

    void FixedUpdate()
    {        
        if (isKnockback)
        {
            return;
        }
        rb2D.velocity = movimiento * velocidad;
    }

    public void EndAttack()
    {
        hitsAtaques[animator.GetInteger("Direccion")].SetActive(false);
        atacando = false;        
    }

    public void EndHurt()
    {
        daniado = false;
    }
}

