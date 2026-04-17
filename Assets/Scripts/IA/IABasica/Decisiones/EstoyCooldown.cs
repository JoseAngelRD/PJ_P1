using UnityEngine;

public class EstoyCooldown : Decision
{
    private NodoArbol nodoVerdadero;
    private NodoArbol nodoFalso;
    private CharacterController controller;

    public EstoyCooldown(NodoArbol verdadero, NodoArbol falso, CharacterController controller)
    {
        this.nodoVerdadero = verdadero;
        this.nodoFalso = falso;
        this.controller = controller;
    }

    public override NodoArbol ObtenerRama(GameObject player)
    {
        return controller.cooldownActual > 0 ? nodoVerdadero : nodoFalso;
    }
}