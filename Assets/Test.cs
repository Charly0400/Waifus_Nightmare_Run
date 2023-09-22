using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    private Rigidbody2D rb;
    //private float moveSpeed = 5f;
    [Header ("Jump")]
    [SerializeField] private float jumpSpeed;
    [SerializeField] private bool canJump;

    [Header ("Ground")]
    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private bool isGrounded;
    [Header ("Wall")]
    [SerializeField] private Transform wallCheck;
    [SerializeField] private LayerMask wallLayer;
    [SerializeField] private bool isTouchingWall;

    [SerializeField] private float radius;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        // Checa si está en el suelo
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, radius, groundLayer);

        // Checa si está chocando 
        isTouchingWall = Physics2D.OverlapCircle(wallCheck.position, radius, wallLayer);

        // Habilita el salto si está en el suelo o tocando una pared
        canJump = isGrounded || isTouchingWall;

        if (Input.GetKeyDown(KeyCode.Space) && canJump)
        {
            Jump();
        }
    }

    private void Jump()
    {
        rb.velocity = new Vector2(rb.velocity.x, jumpSpeed);
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(groundCheck.position, radius);
        Gizmos.DrawWireSphere(wallCheck.position, radius);
    }
}