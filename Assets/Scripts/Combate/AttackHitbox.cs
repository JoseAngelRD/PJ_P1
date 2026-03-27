using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackHitbox : MonoBehaviour
{
    [SerializeField] float damage = 1;
    [SerializeField] LayerMask targetMask;

    void Start()
    {
        Debug.Log("StartAttack");
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("TriggerEnter");
        if ((targetMask.value & (1 << other.gameObject.layer)) == 0)
            return;

        DamageReceiver dmg = other.GetComponent<DamageReceiver>();
        
        if (dmg != null)
        {
            Debug.Log("Player encontrado");
            dmg.RecibirDanio(damage, transform.position);
        }
    }

    public void SetDamage(float damage)
    {
        this.damage = damage;
    }
}
