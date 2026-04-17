using System.Collections;
using UnityEngine;

public class DashAtaque : Accion
{
    private CharacterController controller;

    public DashAtaque(CharacterController controller)
    {
        this.controller = controller;
    }

    public override void EjecutarAccion(GameObject player)
    {
        controller.StartCoroutine(controller.EjecutarConGuardia(Atacar(player), 1f));
    }

    private IEnumerator Atacar(GameObject player)
    {
        // Apuntar al player
        controller.Apuntar(player, true); 

        controller.Atacar(1);
        yield return new WaitUntil(() => !controller.atacando);
    }
}