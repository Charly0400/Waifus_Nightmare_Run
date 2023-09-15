using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class player_Movement : MonoBehaviour
{
    [SerializeField]
    private Transform groundCheck;
    [SerializeField]
    private LayerMask ground;
    [SerializeField]
    private float radius;
    [SerializeField]
    private float jumpSpeed;

    
    [SerializeField]
    private float moveSpeed = 5f;
    private Rigidbody2D rb;
    bool mR = false;
    bool mL = false;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }
    public void Update()
    {
        
    }


    public void MoveLeft()
    {
        mR = true;
        rb.velocity = new Vector2(-moveSpeed, rb.velocity.y);
    }

    public void MoveRight()
    {
        rb.velocity = new Vector2(moveSpeed, rb.velocity.y);
    }

    public void StopMoving()
    {
        rb.velocity = new Vector2(0f, rb.velocity.y);
    }

    public void Jump()
    {
        bool isGrounded = Physics2D.OverlapCircle(groundCheck.position, radius, ground);

        if (isGrounded)
            rb.AddForce(new Vector2(0, jumpSpeed));
    }
   

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(groundCheck.position, radius);
    }
}