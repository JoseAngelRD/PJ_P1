using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class NieveController : CharacterController
{    
    // Update is called once per frame
    void Update()
    {        
        if (atacando)
        {
            movimiento = Vector2.zero;
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
            Atacar(0);
        }
    }

    void FixedUpdate()
    {
        rb2D.velocity = new Vector2(movimiento.x * velocidad, rb2D.velocity.y);
    }

    protected override void Atacar(int id)
    {
        switch(id)
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
    }
}

