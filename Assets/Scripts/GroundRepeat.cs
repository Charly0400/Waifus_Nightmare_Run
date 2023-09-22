using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundRepeat : MonoBehaviour
{
    private float spriteWidht;

    // Start is called before the first frame update
    void Start()
    {
        Transform groundCollider = GetComponent<Transform>();
        spriteWidht = groundCollider.localScale.x;
    }

    // Update is called once per frame
    void Update()
    {
        if(transform.position.x < -spriteWidht)
        {
            transform.Translate(Vector2.right * 2f * spriteWidht);
        }
        
    }
}
