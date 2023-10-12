using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arbusto : MonoBehaviour
{
    public Transform radiusCheck;
    public Transform positionCheck;
    public Transform targetPosition; // Transform del objeto al que deseas llegar
    public float fuerzaDeLanzamiento = 10f;
    public float radioDeActivacion = 5f;
    public float radioPosition = 5f;
    private bool lanzado = false;
    private Rigidbody2D rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        GameObject jugador = GameObject.FindGameObjectWithTag("Player");
        float distanciaAlJugador = Vector2.Distance(transform.position, jugador.transform.position);

        if (distanciaAlJugador < radioDeActivacion && !lanzado)
        {
            Lanzar();
        }
    }

    private void Lanzar()
    {
        rb.isKinematic = false;

        // Calcula la dirección hacia el objetivo
        Vector2 direccion = (targetPosition.position - transform.position).normalized;

        // Aplica una fuerza en la dirección deseada
        rb.AddForce(direccion * fuerzaDeLanzamiento, ForceMode2D.Impulse);

        lanzado = true;
    }

    private void FixedUpdate()
    {
        if (lanzado)
        {
            float distanciaAlObjetivo = Vector2.Distance(transform.position, positionCheck.position);

            if (distanciaAlObjetivo <= radioPosition)
            {
                rb.velocity = Vector2.zero; // Detiene el movimiento actual
                rb.isKinematic = true; // Haz que el Rigidbody2D sea kinemático para detener su movimiento
                lanzado = false; // El objeto se ha detenido
            }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(radiusCheck.position, radioDeActivacion);
        Gizmos.DrawWireSphere(positionCheck.position, radioPosition);
    }
}
