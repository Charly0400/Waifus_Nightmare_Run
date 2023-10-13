using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camera_Follow : MonoBehaviour
{
    public Transform target; // El objetivo que la c�mara seguir� (el personaje)
    public float smoothSpeed = 5f; // La velocidad de seguimiento suave

    private Vector3 desiredPosition; // La posici�n deseada de la c�mara

    void Update()
    {
        // Calcula la posici�n deseada de la c�mara
        desiredPosition = target.position;

        // Utiliza SmoothDamp para suavizar el movimiento de la c�mara
        transform.position = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed * Time.deltaTime);
    }
}