using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camera_Follow : MonoBehaviour
{
    public Transform target; // El objetivo que la cámara seguirá (el personaje)
    public float smoothSpeed = 5f; // La velocidad de seguimiento suave

    private Vector3 desiredPosition; // La posición deseada de la cámara

    void Update()
    {
        // Calcula la posición deseada de la cámara
        desiredPosition = target.position;

        // Utiliza SmoothDamp para suavizar el movimiento de la cámara
        transform.position = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed * Time.deltaTime);
    }
}