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

        Vector3 centroBoss = new Vector3(boss.transform.position.x, boss.transform.position.y-0.2f,boss.transform.position.z);
        Debug.DrawRay(centroBoss, Vector3.right * alcanceDash, Color.blue);
        Debug.DrawRay(centroBoss, Vector3.left * alcanceDash, Color.blue);

        if (Mathf.Abs(deltaX) <= alcanceDash)
        {
            return nodoVerdadero;
        }                 
        return nodoFalso;
    }
}