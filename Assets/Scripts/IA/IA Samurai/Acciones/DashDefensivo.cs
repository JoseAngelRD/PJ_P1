using System.Collections;
using UnityEngine;

public class DashDefensivo : Accion
{
    private CharacterController controller;

    public DashDefensivo(CharacterController controller)
    {
        this.controller = controller;
    }

    public override void EjecutarAccion(GameObject player)
    {
        controller.StartCoroutine(controller.EjecutarConGuardia(Accion(player), 0.2f));
    }

    private IEnumerator Accion(GameObject player)
    {
        // Se mueve en dirección contraria al jugador
        controller.Apuntar(player, false);
        
        controller.HacerDash();
        yield return new WaitUntil(() => !controller.isDashing);
    }
}