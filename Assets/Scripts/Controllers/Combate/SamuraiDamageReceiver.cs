using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SamuraiDamageReceiver : DamageReceiver
{
    Animator animator;
    Rigidbody2D rb;

    private void Start()
    {
        animator = GetComponentInParent<Animator>();
        rb = GetComponentInParent<Rigidbody2D>();
    }

    protected override void ReaccionAlDanio(Vector2 origen)
    {
        animator.SetTrigger("Hurt");

        /*Vector2 direccion = (transform.position - (Vector3)origen).normalized;
        rb.velocity = Vector2.zero;
        rb.AddForce(direccion * 5f, ForceMode2D.Impulse);
        StartCoroutine(KnockbackRoutine());*/

        GetComponentInParent<CharacterController>().daniado = true;
    }

    IEnumerator KnockbackRoutine()
    {
        GetComponentInParent<CharacterController>().isKnockback = true;

        yield return new WaitForSeconds(0.2f);

        GetComponentInParent<CharacterController>().isKnockback = false;
    }

    protected override void Morir()
    {
        animator.SetTrigger("Death");
        FindObjectOfType<Cronometro>().DetenerYComprobarRecord("Samurai");
    }
}