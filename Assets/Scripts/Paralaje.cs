using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Timeline;

public class Paralaje : MonoBehaviour
{
    private List<Transform> capas = new List<Transform>();
    [SerializeField] private GameObject player;
    private Rigidbody2D rbPlayer;

    [SerializeField] private float velocidad;

    void Start()
    {
        rbPlayer = player.GetComponent<Rigidbody2D>();
        foreach (Transform hijo in transform)
        {
            capas.Add(hijo);
        }
    }

    // Update is called once per frame
    void Update()
    {
        float velocidadJugador = rbPlayer.velocity.x;
        int direccion = 0;
        if (velocidadJugador > 0)
        {
            direccion = -1;
        } else if (velocidadJugador < 0)
        {
            direccion = 1;
        }

        foreach (Transform c in capas)
        {
            SpriteRenderer sr = c.GetComponent<SpriteRenderer>();

            if (sr != null)
            {
                float factor = (capas.Count - sr.sortingOrder) * velocidad;

                c.Translate(new Vector3(factor * Time.deltaTime * direccion, 0, 0));
            }
        }
    }
}
