using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [SerializeField] GameObject[] obstacles;
    [SerializeField] private float minTime = 0.6f;
    [SerializeField] private float maxTime = 2f;

    private void Start()
    {
        StartCoroutine(SpawnObstacles());
    }

    public void Update()
    {
        UpdateTime();
    }
    private IEnumerator SpawnObstacles()
    {
        while (true)
        {
           int randomIndex = Random.Range(0, obstacles.Length);
        float randomTime = Random.Range(minTime, maxTime);

        // Obtener la rotaci�n original del obst�culo
        Quaternion originalRotation = obstacles[randomIndex].transform.rotation;

        // Obtener la posici�n original del obst�culo
        Vector3 originalPosition = obstacles[randomIndex].transform.position;

        // Asegurar que la posici�n en X del objeto instanciado sea la misma que la del spawner
        Vector3 newPosition = new Vector3(transform.position.x, originalPosition.y, originalPosition.z);

        // Instanciar el obst�culo con la posici�n y rotaci�n originales
        GameObject newObstacle = Instantiate(obstacles[randomIndex], newPosition, originalRotation);

        yield return new WaitForSeconds(randomTime);
        }
    }

    public void UpdateTime()
    {
        //float time = 10f;

    }
 
}
