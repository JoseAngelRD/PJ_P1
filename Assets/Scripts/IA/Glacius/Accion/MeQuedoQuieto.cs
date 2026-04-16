using UnityEngine;

public class MeQuedoQuieto : Accion
{
    private CharacterController controller;

    public MeQuedoQuieto(CharacterController controller)
    {
        this.controller = controller;
    }

    public override void EjecutarAccion(GameObject player)
    {
        controller.SetMovimiento(Vector2.zero);
    }
}