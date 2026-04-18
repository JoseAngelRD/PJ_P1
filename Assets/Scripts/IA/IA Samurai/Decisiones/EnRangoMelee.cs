using UnityEngine;

public class EnRangoMelee : Decision
{
    private NodoArbol nodoVerdadero;
    private NodoArbol nodoFalso;
    private GameObject boss;

    public EnRangoMelee(NodoArbol verdadero, NodoArbol falso, GameObject boss)
    {
        this.nodoVerdadero = verdadero;
        this.nodoFalso = falso;
        this.boss = boss;        
    }

    public override NodoArbol ObtenerRama(GameObject player)
    {
        foreach (Collider2D colision in boss.GetComponentsInChildren<Collider2D>())
        {
            if (colision.gameObject.layer == LayerMask.NameToLayer("EnemyHit"))
            {
                BoxCollider2D box = colision as BoxCollider2D;
                if (box == null) continue;

                // escala global (importante)
                float scaleX = colision.transform.lossyScale.x;
                // centro en mundo (teniendo en cuenta flip)
                float centroX = colision.transform.position.x + box.offset.x * scaleX;
                // mitad del tamaño real
                float rango = (box.size.x * Mathf.Abs(scaleX)) / 2f;
                float centroBoss = boss.transform.position.x;
                // extremos reales del collider
                float min = centroX - rango;
                float max = centroX + rango;

                // rango máximo desde el boss
                float rangoFinal = Mathf.Max(
                    Mathf.Abs(max - centroBoss),
                    Mathf.Abs(min - centroBoss)
                );

                float distanciaPlayer = Mathf.Abs(player.transform.position.x - centroBoss);                
                
                Vector3 centro = new Vector3(centroX, colision.transform.position.y, 0f);
                Debug.DrawRay(centro, Vector3.right * rango, Color.green);
                Debug.DrawRay(centro, Vector3.left * rango, Color.green);

                if (distanciaPlayer <= rangoFinal)
                {                    
                    return nodoVerdadero;
                }                
            }
        }               
        return nodoFalso;
    }
}