using UnityEngine;

public class MenosMitadVida : Decision
{
    private NodoArbol nodoVerdadero;
    private NodoArbol nodoFalso;
    private CharacterController controller;

    public MenosMitadVida(NodoArbol verdadero, NodoArbol falso, CharacterController controller)
    {
        this.nodoVerdadero = verdadero;
        this.nodoFalso = falso;
        this.controller = controller;
    }

    public override NodoArbol ObtenerRama(GameObject player)
    {
        bool menosMitad = controller.vidaActual < (controller.vidaMaxima / 2);
        return menosMitad ? nodoVerdadero : nodoFalso;
    }
}