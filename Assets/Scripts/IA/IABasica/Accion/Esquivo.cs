using UnityEngine;

public class Esquivo : Accion
{
    private CharacterController controller;

    public Esquivo(CharacterController controller)
    {
        this.controller = controller;
    }

    public override void EjecutarAccion(GameObject player)
    {
        // Se mueve en dirección contraria al jugador
        Vector2 direccionAlejamiento = (controller.transform.position - player.transform.position).normalized;
        controller.SetMovimiento(new Vector2(Mathf.Sign(direccionAlejamiento.x), 0));
    }
}