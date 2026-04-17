using System.Collections;
using UnityEngine;

public class Esperar : Accion
{
    private CharacterController controller;
    private float tiempo;

    public Esperar(CharacterController controller, float tiempo)
    {
        this.controller = controller;
        this.tiempo = tiempo;
    }

    public override void EjecutarAccion(GameObject player)
    {
        controller.StartCoroutine(controller.EjecutarConGuardia(Espera(player), 0f));
    }

    private IEnumerator Espera(GameObject player)
    {
        yield return new WaitForSeconds(tiempo);
    }
}