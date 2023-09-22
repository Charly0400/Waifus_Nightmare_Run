using System.Collections;
using System.Collections.Generic;
using UnityEditor.Tilemaps;
using UnityEngine;

public class player_Movement : MonoBehaviour
{
    private Rigidbody2D rb;
    [SerializeField] private float radius;
    //private float moveSpeed = 5f;

    public float horizontal;
    public bool isFacingright = true;

    [Header("Jump")]
    [SerializeField] private float jumpSpeed;
    [SerializeField] private bool canJump;
    [SerializeField] private float speed;


    [Header("Ground")]
    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private bool isGrounded;

    [Header("Wall")]
    [SerializeField] private Transform wallCheck;
    [SerializeField] private LayerMask wallLayer;
    [SerializeField] private bool isTouchingWall;

    [Header("WallSliding & Wall Jumping")]
    [SerializeField] private bool isWallSliding;
    [SerializeField] private float wallSlidingSpeed;
    [SerializeField] private bool isWallJumping;
    private float wallJumpingDirection;
    [SerializeField] private float wallJumpingTime =.2f;
    [SerializeField] private float wallJumpingCounter;
    [SerializeField] private float wallJumpingDuration = .4f;
    [SerializeField] private Vector2 wallJumpingPower = new Vector2(0f, 16f);





    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        // Checa si está en el suelo
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, radius, groundLayer);

        // Checa si está chocando con una pared
        isTouchingWall = Physics2D.OverlapCircle(wallCheck.position, radius, wallLayer);

        // Habilita el salto si está en el suelo o tocando una pared
        canJump = isGrounded || isTouchingWall;

        if (Input.GetKeyDown(KeyCode.Space) && canJump)
        {
            Jump();
        }

        WallSlide();
        WallJump();

        if (!isWallJumping)
        {
            Flip();
        }
    }
    private void FixedUpdate()
    {
        if (!isWallJumping)
        {
            rb.velocity = new Vector2(horizontal * speed, rb.velocity.y);
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

    private void  WallSlide()
    {
        if (isTouchingWall && !isGrounded)
        {
            isWallSliding = true;
            rb.velocity = new Vector2(rb.velocity.x, Mathf.Clamp(rb.velocity.y, -wallSlidingSpeed, float.MaxValue));
    
        }
        else
        {
            isWallSliding = false;

        }
        
    }
    private void WallJump()
    {
        if (isWallSliding)
        {
            isWallJumping = false;
            wallJumpingDirection = -transform.localScale.x;
            wallJumpingCounter = wallJumpingTime;

            CancelInvoke(nameof(StopWallJumping));
        }
        else
        {
            wallJumpingCounter -= Time.deltaTime;
        }
        if ( Input.GetKeyDown(KeyCode.Space) && wallJumpingCounter > 0f)
        {
            isWallJumping = true;
            rb.velocity = new Vector2(wallJumpingDuration * wallJumpingPower.x, wallJumpingPower.y);
            wallJumpingCounter = 0f;
            
            if(transform.localScale.x != wallJumpingDirection)
            {
                isFacingright = !isFacingright;
                Vector3 localScale = transform.localScale;
                localScale.x *= -1f;
                transform.localScale = localScale;
            }

        Invoke(nameof(StopWallJumping), wallJumpingDirection);
        
        }
    }
    private void StopWallJumping()
    {
        isWallJumping = false;
    }

    private void Flip()
    {
        if(isFacingright && horizontal <0f || !isFacingright && horizontal > 0f)
        {
            isFacingright = !isFacingright;
            Vector3 localscale = transform.localScale;
            localscale.x *= -1f;
            transform.localScale= localscale;
        }
    }
}