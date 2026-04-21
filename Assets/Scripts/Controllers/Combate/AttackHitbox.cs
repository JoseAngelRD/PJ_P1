using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackHitbox : MonoBehaviour
{
    private float damage = 1;
    [SerializeField] LayerMask targetMask;

    void OnTriggerEnter2D(Collider2D other)
    {        
        if (((1 << other.gameObject.layer) & targetMask) == 0)
            return;

        var dmg = other.GetComponentInParent<DamageReceiver>();

        if (dmg != null)
        {
            dmg.RecibirDanio(damage, transform.position);
        }
    }

    public void SetDamage(float damage)
    {
        this.damage = damage;
    }
}

