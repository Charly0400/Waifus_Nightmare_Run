using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class player_Movement : MonoBehaviour
{
    private Rigidbody2D rb;
    //private float moveSpeed = 5f;
    [Header("Jump")]
    [SerializeField] private float jumpSpeed;
    [SerializeField] private bool canJump;

    [Header("Ground")]
    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private bool isGrounded;
    [Header("Wall")]
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
        // Checa si est� en el suelo
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, radius, groundLayer);

        // Checa si est� chocando con una pared
        isTouchingWall = Physics2D.OverlapCircle(wallCheck.position, radius, wallLayer);

        // Habilita el salto si est� en el suelo o tocando una pared
        canJump = isGrounded || isTouchingWall;

        if (Input.GetKeyDown(KeyCode.Space) && canJump)
        {
            Jump();
        }
    }

    public void Jump()
    {
        if (isGrounded)
            rb.AddForce(new Vector2(0, jumpSpeed));
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(groundCheck.position, radius);
        Gizmos.DrawWireSphere(wallCheck.position, radius);
    }
}