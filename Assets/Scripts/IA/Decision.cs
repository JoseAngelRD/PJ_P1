using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Decision : NodoArbol
{
    public abstract NodoArbol ObtenerRama(GameObject player);

    public override NodoArbol Decide(GameObject player)
    {
        return ObtenerRama(player).Decide(player);
    }
}
