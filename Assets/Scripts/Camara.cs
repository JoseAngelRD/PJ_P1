using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camara : MonoBehaviour
{
    [SerializeField] private Transform player;
    [SerializeField] private float deadZone = 2f;
    [SerializeField] private float suavizado = 0.1f;

    private float velocidadRef = 0f;

    void Update()
    {
        float diffX = player.position.x - transform.position.x;

        if (Mathf.Abs(diffX) > deadZone)
        {
            float objetivoX = player.position.x - Mathf.Sign(diffX) * deadZone;

            float nuevaX = Mathf.SmoothDamp(
                transform.position.x,
                objetivoX,
                ref velocidadRef,
                suavizado
            );

            transform.position = new Vector3(nuevaX, transform.position.y, transform.position.z);
        }
    }
}
