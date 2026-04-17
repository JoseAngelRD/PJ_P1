using UnityEngine;

public class EstaEnRango : Decision
{
    private NodoArbol nodoVerdadero;
    private NodoArbol nodoFalso;
    private Transform transformBoss;
    private float rango;

    public EstaEnRango(NodoArbol verdadero, NodoArbol falso, Transform transformBoss, float rango)
    {
        this.nodoVerdadero = verdadero;
        this.nodoFalso = falso;
        this.transformBoss = transformBoss;
        this.rango = rango;
    }

    public override NodoArbol ObtenerRama(GameObject player)
    {
        Debug.Log("Evaluando distancia al jugador...");
        float distancia = Vector2.Distance(transformBoss.position, player.transform.position);
        return distancia <= rango ? nodoVerdadero : nodoFalso;
    }
}