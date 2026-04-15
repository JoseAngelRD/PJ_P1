using System;
using UnityEngine;

public class PlayerController : CharacterController
{        
    // Update is called once per frame
    void Update()
    {
        // Estado del player
        Debug.DrawRay(transform.position, Vector3.down * tamRaycast, Color.red);
        RaycastHit2D colisionDown = Physics2D.Raycast(transform.position, Vector3.down, tamRaycast, groundLayer);
        if (colisionDown != false && colisionDown.collider.CompareTag("Suelo")) onGround = true;
        else onGround = false;

        // Prohibir el movimento al ser dañado (animacion de daño + knockback actuando)
        if (daniado)
        {
            movimiento = Vector2.zero;
            return;
        }
        // Prohibir el movimiento al atacar en el suelo
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

        // Comprobar si se esta protegiendo
        if (Input.GetMouseButton(1)) {
            //animator.SetBool("Shield", true);
            shield = true;      
        } else
        {
            //animator.SetBool("Shield", false);
            shield = false;
        }
        // Cancelar el movimiento si se esta protegiendo en el suelo
        if (shield)
        {
            if (onGround) movimiento = Vector2.zero;
            return;
        }
        
        movimiento.x = Input.GetAxisRaw("Horizontal");
        movimiento = movimiento.normalized;

        if (movimiento.x < 0.0f) transform.localScale = new Vector3 (-Math.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
        else if (movimiento.x > 0.0f) transform.localScale = new Vector3 (Math.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);        
                
        animator.SetFloat("Horizontal", movimiento.x);        
        animator.SetFloat("Speed", movimiento.magnitude);

        if (Input.GetMouseButtonDown(0)) {            
            Atacar(0);           
        }        

        if ((Input.GetKey(KeyCode.Space) || Input.GetKey(KeyCode.W)) && onGround && rb2D.velocity.y < 0.1f)
        {            
            Saltar();        
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

    protected override void Atacar(int id)
    {
        switch (id)
        {
            case 0:
            {
                Debug.Log("LeftClick");
                animator.SetTrigger("Attack");
                atacando = true;         
            }
            break;
        }
    }
}

