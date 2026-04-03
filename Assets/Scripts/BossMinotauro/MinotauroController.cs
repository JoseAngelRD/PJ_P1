using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MinotauroController : MonoBehaviour
{
    [SerializeField] private float velocidad = 5.0f;
    private Rigidbody2D rb2D;
    private Vector2 movimiento;
    private Animator animator;

    // Variables de salto
    [SerializeField] private LayerMask groundLayer;

    // Variables de combate
    private bool atacando = false;
    [SerializeField] List<GameObject> hitsAtaques;
    [SerializeField] private float attackDamage;

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

        /*if (daniado)
        {
            movimiento = Vector2.zero;
            return;
        }*/
        if (atacando)
        {
            movimiento = Vector2.zero;
            if (animator.GetCurrentAnimatorStateInfo(0).IsName("Idle"))
            {
                Debug.Log("Cancelado el ataque");
                EndAttack();
            }
            return;
        }

        movimiento.x = Input.GetAxisRaw("Horizontal");
        movimiento = movimiento.normalized;

        if (movimiento.x < 0.0f) transform.localScale = new Vector3(Math.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
        else if (movimiento.x > 0.0f) transform.localScale = new Vector3(-Math.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);

        animator.SetFloat("Horizontal", movimiento.x);
        animator.SetFloat("Speed", movimiento.magnitude);

        if (Input.GetMouseButtonDown(0))
        {
            Debug.Log("LeftClick");
            animator.SetTrigger("Attack");
            atacando = true;
        }

        if (Input.GetKey(KeyCode.A))
        {
            animator.SetInteger("Direccion", 0);
        }
        else if (Input.GetKey(KeyCode.D))
        {
            animator.SetInteger("Direccion", 1);
        }
    }

    void FixedUpdate()
    {
        rb2D.velocity = new Vector2(movimiento.x * velocidad, rb2D.velocity.y);
    }

    public void ActivarHit(int index)
    {
        hitsAtaques[index].SetActive(true);
    }

    public void EndHit()
    {
        foreach (GameObject h in hitsAtaques)
        {
            h.SetActive(false);
        }
    }

    public void EndAttack()
    {
        atacando = false;
    }
}

