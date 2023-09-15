using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : MonoBehaviour
{
    public Rigidbody2D rb;
    public Transform player;

    private bool mirandoDerecha = true;

    [Header("Vida")]

    [SerializeField]
    private float vida;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();

    }

    public void MirarJugador()
    {
        if((player.position.x > transform.position.x && !mirandoDerecha) ||
           (player.position.x < transform.position.x && mirandoDerecha))
        {
            mirandoDerecha = !mirandoDerecha;
            transform.eulerAngles = new Vector3(0, transform.eulerAngles.y + 100, 0);
        }
    }
}
