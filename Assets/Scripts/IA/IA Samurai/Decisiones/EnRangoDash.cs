using UnityEngine;

public class EnRangoDash : Decision
{
    private NodoArbol nodoVerdadero;
    private NodoArbol nodoFalso;
    private GameObject boss;

    public EnRangoDash(NodoArbol verdadero, NodoArbol falso, GameObject boss)
    {
        this.nodoVerdadero = verdadero;
        this.nodoFalso = falso;
        this.boss = boss;        
    }

    public override NodoArbol ObtenerRama(GameObject player)
    {
        CharacterController bossControl = boss.GetComponent<CharacterController>();            
        float deltaX = player.transform.position.x - boss.transform.position.x;        
        float alcanceDash = bossControl.dashForce * bossControl.dashDuration;
        
        // Solo comprobar distancia, no dirección (el flip ocurre después en Update)
        if (Mathf.Abs(deltaX) <= alcanceDash)
        {
            return nodoVerdadero;
        }                 
        return nodoFalso;
    }
}