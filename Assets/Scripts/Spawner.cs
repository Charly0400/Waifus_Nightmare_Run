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

    private IEnumerator SpawnObstacles()
    {
        while (true)
        {
            int randomIndex = Random.Range(0, obstacles.Length);
            float randomTime = Random.Range(minTime, maxTime);

            Instantiate(obstacles[randomIndex], transform.position, Quaternion.identity);
           yield return new WaitForSeconds(randomTime);
        }

    }

 
}
