using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
public GameObject[] gameObjects;


    private float Timer;
    private float initialWait;

    private void Awake()
    {
        Timer = 3;
        initialWait = 2;
    }

    private void Update()
    {
        Timer -= Time.deltaTime;
        if(Timer <=0)
            Instantiate(gameObject);
    }

}
