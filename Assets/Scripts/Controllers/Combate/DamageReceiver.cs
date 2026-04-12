using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageReceiver : MonoBehaviour
{
    protected float vida;
    [SerializeField] protected float vidaMaxima = 100;

    [SerializeField] protected RectTransform vidaUI = null;    

    void Awake()
    {
        vida = vidaMaxima;
    }

    public virtual void RecibirDanio(float cantidad, Vector2 origen)
    {
        if (vida > 0)
        {
            vida -= cantidad;
            vidaUI.localScale = new Vector3(vida / vidaMaxima, vidaUI.localScale.y, vidaUI.localScale.z);
        
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
        Debug.Log(gameObject.name + " muerto");
    }

    protected virtual void ReaccionAlDanio(Vector2 origen)
    {
        
    }
}