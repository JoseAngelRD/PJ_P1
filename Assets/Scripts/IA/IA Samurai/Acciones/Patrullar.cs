using System.Collections;
using UnityEngine;

public class Patrullar : Accion
{
    private CharacterController controller;    

    public Patrullar(CharacterController controller)
    {
        this.controller = controller;        
    }

    public override void EjecutarAccion(GameObject player)
    {
        controller.StartCoroutine(controller.EjecutarConGuardia(Patrulla(player), 0.5f));
    }

    private IEnumerator Patrulla(GameObject player)
    {
        if (Random.value < 0.5f)
        {
            // Dirección hacia el jugador
            Vector2 direccion = (player.transform.position - controller.transform.position).normalized;
            
            // Solo nos interesa movernos en X en un juego 2D de plataformas
            controller.SetMovimiento(new Vector2(Mathf.Sign(direccion.x), 0));
        } else
        {
            // Se mueve en dirección contraria al jugador
            Vector2 direccionAlejamiento = (controller.transform.position - player.transform.position).normalized;
            controller.SetMovimiento(new Vector2(Mathf.Sign(direccionAlejamiento.x), 0));
        }
        yield return new WaitForSeconds(0.5f);
        controller.SetMovimiento(Vector2.zero);
    }
}