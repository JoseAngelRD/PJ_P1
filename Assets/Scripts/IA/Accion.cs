using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Accion : NodoArbol
{
    public abstract void EjecutarAccion(GameObject player);
    public override NodoArbol Decide(GameObject player)
    {        
        return this;
    }
}
