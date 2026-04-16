using UnityEngine;

public class MeAtaca : Decision
{
    private NodoArbol nodoVerdadero;
    private NodoArbol nodoFalso;

    public MeAtaca(NodoArbol verdadero, NodoArbol falso)
    {
        this.nodoVerdadero = verdadero;
        this.nodoFalso = falso;
    }

    public override NodoArbol ObtenerRama(GameObject player)
    {
        PlayerController playerCtrl = player.GetComponent<PlayerController>();

        bool meEstanAtacando = playerCtrl != null && playerCtrl.atacando; 
        
        return meEstanAtacando ? nodoVerdadero : nodoFalso;
    }
}