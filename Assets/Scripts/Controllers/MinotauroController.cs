using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MinotauroController : CharacterController
{
    [SerializeField] private Transform player;
    private int direccionHuida = 0;
    
    // Añadimos estas variables para el cronómetro
    private Cronometro cronometro;

    void Start()
    {
        // Buscamos el cronómetro en la escena al empezar
        cronometro = FindObjectOfType<Cronometro>();
    }

    // Update is called once per frame
    void Update()
    {        
        Debug.Log(GetComponent<SpriteRenderer>().color);
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
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("Death"))
        {            
            // LÓGICA DE HUIDA
            if (direccionHuida == 0)
            {
                direccionHuida = Math.Sign(transform.position.x-player.position.x);
                Debug.Log(direccionHuida);
                if (direccionHuida == 0)
                {
                    direccionHuida = 1;
                }
            }
            transform.localScale = new Vector3(Math.Abs(transform.localScale.x)*-direccionHuida, transform.localScale.y, transform.localScale.z);
            rb2D.velocity = new Vector2(direccionHuida*10, 0);
        } 
        else
        {
            rb2D.velocity = new Vector2(movimiento.x * velocidad, rb2D.velocity.y);
        }
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