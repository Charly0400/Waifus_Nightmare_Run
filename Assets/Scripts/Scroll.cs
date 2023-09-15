using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scroll : MonoBehaviour
{
    public float scrollSpeed = 8f;
    void Update()
    {
        transform.Translate(Vector2.left * scrollSpeed * Time.deltaTime);
    }
}
