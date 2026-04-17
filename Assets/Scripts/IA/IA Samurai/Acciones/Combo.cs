using System.Collections;
using UnityEngine;

public class Combo : Accion
{
    private CharacterController controller;

    public Combo(CharacterController controller)
    {
        this.controller = controller;
    }

    public override void EjecutarAccion(GameObject player)
    {
        controller.StartCoroutine(controller.EjecutarConGuardia(ComboAtaque(player), 1f));
    }

    private IEnumerator ComboAtaque(GameObject player)
    {
        // Apunto al player
        controller.Apuntar(player, true);        

        // Ataque con dash
        controller.SetMovimiento(Vector2.zero); // Se detiene para pegar
        controller.Atacar(1);
        yield return new WaitUntil(() => !controller.atacando);

        // Vuelvo a apuntar al player
        controller.Apuntar(player, true); 

        // Ataque basico
        controller.SetMovimiento(Vector2.zero);
        controller.Atacar(0);
    }
}