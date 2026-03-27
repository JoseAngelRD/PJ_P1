using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageReceiver : MonoBehaviour
{
    [SerializeField] protected float vida = 5;

    public virtual void RecibirDanio(float cantidad, Vector2 origen)
    {
        vida -= cantidad;

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

    protected virtual void Morir()
    {
        Debug.Log(gameObject.name + " muerto");
    }

    protected virtual void ReaccionAlDanio(Vector2 origen)
    {
        
    }
}
