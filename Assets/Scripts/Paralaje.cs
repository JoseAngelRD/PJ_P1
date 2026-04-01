using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Paralaje : MonoBehaviour
{
    [SerializeField] private List<GameObject> capas = new List<GameObject>();
    [SerializeField] private GameObject player;
    private Rigidbody2D rbPlayer;

    [SerializeField] private float velocidad;

    void Start()
    {
        rbPlayer = player.GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {         
        
        int direccion = 0;
        if (rbPlayer.velocity.x > 0)
        {
            direccion = -1;
        } else if (rbPlayer.velocity.x < 0)
        {
            direccion = 1;
        }

        foreach (GameObject c in capas)
        {
            c.transform.Translate(new Vector3((capas.Count-c.GetComponent<SpriteRenderer>().sortingOrder)*velocidad*direccion, 0, 0));
        }
    }
}
