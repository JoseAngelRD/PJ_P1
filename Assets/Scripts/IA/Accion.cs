using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Accion : NodoArbol
{
    public abstract void EjecutarAccion(GameObject player);
    public override NodoArbol Decide(GameObject player)
    {
        if (this is DashAtaque || this is DashDefensivo || this is DashOfensivo || this is Combo)
        {
            Debug.Log(this);    
        }
        return this;
    }
}
