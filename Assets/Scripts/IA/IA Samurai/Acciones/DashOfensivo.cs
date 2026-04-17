using System.Collections;
using UnityEngine;

public class DashOfensivo : Accion
{
    private CharacterController controller;

    public DashOfensivo(CharacterController controller)
    {
        this.controller = controller;
    }
    public override void EjecutarAccion(GameObject player)
    {
        controller.StartCoroutine(controller.EjecutarConGuardia(Accion(player), 0.2f));
    }

    private IEnumerator Accion(GameObject player)
    {
        // Se mueve en direccion al jugador
        controller.Apuntar(player, true);
        
        controller.HacerDash();
        yield return new WaitUntil(() => !controller.isDashing);
    }
}