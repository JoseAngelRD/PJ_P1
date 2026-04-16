using System;
using UnityEngine;

public class SamuraiController : CharacterController
{        
    // Update is called once per frame
    void Update()
    {
        // Estado del player
        Debug.DrawRay(transform.position, Vector3.down * tamRaycast, Color.red);
        RaycastHit2D colisionDown = Physics2D.Raycast(transform.position, Vector3.down, tamRaycast, groundLayer);
        if (colisionDown != false) onGround = true;
        else onGround = false;

        if (daniado)
        {
            movimiento = Vector2.zero;
            return;
        }
        if (atacando)
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

        if (Input.GetMouseButtonDown(0) && onGround) {            
            Atacar(0);           
        }

        if (Input.GetMouseButtonDown(1) && onGround) {            
            Atacar(1);
        }     

        if (Input.GetKeyDown(KeyCode.LeftShift) && onGround && !isDashing) {            
            StartCoroutine(Dash());            
        }   

        if ((Input.GetKey(KeyCode.Space) || Input.GetKey(KeyCode.W)) && onGround && rb2D.velocity.y < 0.1f)
        {            
            Saltar();            
        }
    }

    void FixedUpdate()
    {        
        if (isKnockback || isDashing)
        {
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
                Debug.Log("LeftClick");
                animator.SetTrigger("Attack1");
                atacando = true; 
            }
            break;
            case 1:
            {
                Debug.Log("RightClick");            
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
            Debug.Log("Minotauro derrotado, deteniendo cronómetro...");
            crono.DetenerYComprobarRecord("Minotauro");
            Debug.Log("detenerYComprobarRecord ejecutado");
        }
        menuMuerte.SetActive(true);
    }
}