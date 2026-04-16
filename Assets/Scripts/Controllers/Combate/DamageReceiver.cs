using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageReceiver : MonoBehaviour
{

    Rigidbody2D rb;
    Animator animator;
    protected float vida;
    protected float vidaCooldown = 0f;
    [SerializeField] protected float vidaMaxima = 100;

    [SerializeField] public float fuerzaKnockback = 7f;
    [SerializeField] protected RectTransform vidaUI = null;

    void Start()
    {
        rb = GetComponentInParent<Rigidbody2D>();
        animator = GetComponentInParent<Animator>();
    }

    void Awake()
    {
        vida = vidaMaxima;
    }

    public virtual void RecibirDanio(float cantidad, Vector2 origen)
    {
        if (vida > 0)
        {
            vida -= cantidad;
            if (vida >= 0)
            {
                vidaUI.localScale = new Vector3(vida / vidaMaxima, vidaUI.localScale.y, vidaUI.localScale.z);    
            } else
            {
                vidaUI.localScale = new Vector3(0, vidaUI.localScale.y, vidaUI.localScale.z);    
            }
            
        
            Debug.Log(gameObject.name + " vida: " + vida);

            if (vida <= 0)
            {
                Morir();
            }
            else
            {
                ReaccionAlDanio(origen);
            }
        }
    }

    protected virtual void Morir()
    {
        string bossActual = "";
        animator.SetTrigger("Death");
        Debug.Log(gameObject.name + " muerto");

        if (gameObject.name != "Player")
        {
            switch (gameObject.scene.name)
            {
                case "SalaBosque":
                    bossActual = "Minotauro";
                    break;
                case "SalaDarkForest":
                    bossActual = "Samurai";
                    break;
                case "SalaNieve":
                    bossActual = "Nieve";
                    break;
            }

            FindObjectOfType<Cronometro>().DetenerYComprobarRecord(bossActual);
        }
    }

    protected virtual void ReaccionAlDanio(Vector2 origen)
    {

        Vector2 direccion = (transform.position - (Vector3)origen).normalized;
        rb.velocity = Vector2.zero;
        rb.AddForce(new Vector2(direccion.x, 0) * fuerzaKnockback, ForceMode2D.Impulse);
        StartCoroutine(KnockbackRoutine());

        if (transform.parent.name == "Player")
        {
            GetComponentInParent<CharacterController>().EndAttack();
            GetComponentInParent<Animator>().SetTrigger("Hurt");
        }
        else
        {
            if (vidaCooldown >= 0.2 * vidaMaxima)
            {
                GetComponentInParent<CharacterController>().EndAttack();
                GetComponentInParent<Animator>().SetTrigger("Hurt");
                vidaCooldown = 0f;
            }
        }
    }

    public float GetVida()
    {
        return vida;
    }

    public float GetVidaMaxima()
    {
        return vidaMaxima;
    }

    IEnumerator KnockbackRoutine()
    {
        GetComponentInParent<CharacterController>().isKnockback = true;

        yield return new WaitForSeconds(0.2f);

        GetComponentInParent<CharacterController>().isKnockback = false;
    }

}