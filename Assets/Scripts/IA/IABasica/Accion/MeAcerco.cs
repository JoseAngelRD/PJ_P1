using UnityEngine;

public class MeAcerco : Accion
{
    private CharacterController controller;

    public MeAcerco(CharacterController controller)
    {
        this.controller = controller;
    }

    public override void EjecutarAccion(GameObject player)
    {
        // Dirección hacia el jugador
        Vector2 direccion = (player.transform.position - controller.transform.position).normalized;
        
        // Solo nos interesa movernos en X en un juego 2D de plataformas
        controller.SetMovimiento(new Vector2(Mathf.Sign(direccion.x), 0));
    }
}