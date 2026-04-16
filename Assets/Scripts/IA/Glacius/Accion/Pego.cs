using UnityEngine;

public class Pego : Accion
{
    private CharacterController controller;

    public Pego(CharacterController controller)
    {
        this.controller = controller;
    }

    public override void EjecutarAccion(GameObject player)
    {
        controller.SetMovimiento(Vector2.zero); // Se detiene para pegar
        controller.Atacar(0);
    }
}