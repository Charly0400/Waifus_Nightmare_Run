using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arbusto : MonoBehaviour
{
    public Transform radiusCheck;
    public float fuerzaDeLanzamiento = 10f;
    public float radioDeActivacion = 5f;
    public Transform posicionDeReferencia; // La posici�n en la que se detendr� el objeto
    public float tiempoDeEsperaEnElAire = 2f; // Tiempo que el objeto espera en el aire antes de caer
    private bool lanzado = false;

    private void Update()
    {
        // Verificar si el jugador est� cerca
        GameObject jugador = GameObject.FindGameObjectWithTag("Player");
        float distanciaAlJugador = Vector2.Distance(transform.position, jugador.transform.position);

        if (distanciaAlJugador < radioDeActivacion && !lanzado)
        {
            Lanzar();
        }
    }

    private void Lanzar()
    {
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        rb.AddForce(Vector2.up * fuerzaDeLanzamiento, ForceMode2D.Impulse);
        lanzado = true;
        StartCoroutine(EsperarYCaer());
    }

    private IEnumerator EsperarYCaer()
    {
        yield return new WaitForSeconds(tiempoDeEsperaEnElAire);

        // Detener el objeto para que caiga
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        rb.velocity = Vector2.zero;

        // Mover el objeto a la posici�n de referencia
        while (transform.position.y < posicionDeReferencia.position.y)
        {
            transform.Translate(Vector3.up * Time.deltaTime);
            yield return null;
        }

        // El objeto cae despu�s de detenerse en la posici�n de referencia
        rb.isKinematic = false;
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(radiusCheck.position, radioDeActivacion);
    }
}
