using UnityEngine;

public class ProbabilidadDecision : Decision
{
    private NodoArbol nodoVerdadero;
    private NodoArbol nodoFalso;
    private float porcentajeVerdadero; 
    
    // Variables de memoria
    private float tiempoMemoria; 
    private float temporizador = 0f;
    private bool ultimaDecisionVerdadera;

    // Añadimos un parámetro opcional de tiempoMemoria (0.5 segundos por defecto)
    public ProbabilidadDecision(NodoArbol verdadero, NodoArbol falso, float porcentaje, float tiempoMemoria = 0.5f)
    {
        this.nodoVerdadero = verdadero;
        this.nodoFalso = falso;
        this.porcentajeVerdadero = porcentaje;
        this.tiempoMemoria = tiempoMemoria;
    }

    public override NodoArbol ObtenerRama(GameObject player)
    {
        // Reducimos el temporizador solo cuando este nodo está siendo evaluado
        temporizador -= Time.deltaTime;
        
        // Solo volvemos a calcular la probabilidad si el tiempo se agotó
        if (temporizador <= 0f)
        {
            float random = Random.Range(0f, 100f);
            ultimaDecisionVerdadera = (random <= porcentajeVerdadero);
            
            // Reiniciamos el temporizador
            temporizador = tiempoMemoria;
        }
        
        return ultimaDecisionVerdadera ? nodoVerdadero : nodoFalso;
    }
}