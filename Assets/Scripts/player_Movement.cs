using System.Collections;
using System.Collections.Generic;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UI;

public class player_Movement : MonoBehaviour
{
    private Rigidbody2D rb;
    [SerializeField] private float radius;
    //private float moveSpeed = 5f;
    private float originalJumpSpeed; // Almacena la fuerza de salto original
    public float horizontal;
    public bool isFacingright = true;

    [Header("Friccion")]
    [SerializeField] private float friction = 10f;
    [SerializeField] private bool frictionActive = false;
    [SerializeField] private float freno = 10f;
    [SerializeField] private bool frenoActive = false;

    [Header("Reset Position")]
    [SerializeField] private Vector3 initialPosition; 
    [SerializeField] private float resetDistance = 10f; // La distancia para activar el reset
    [SerializeField] private float resetDelay = 2f; // El tiempo de espera antes de realizar el reset
    private float distanceTraveled = 0f;
    private float timeSinceLastReset = 0f;

    [Header("Jump")]
    public bool isJumping = false;
    [SerializeField] private float jumpSpeed;
    [SerializeField] private bool canJump;
    [SerializeField] private float speed;
    [SerializeField] private int maxJumps = 2; // Cantidad m�xima de saltos permitidos
    private int jumpsPerformed = 0; // Cantidad de saltos realizados
    [SerializeField] private float secondJumpForce = 10f; // Fuerza del segundo salto

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
        originalJumpSpeed = jumpSpeed;
    }
    public void Update()
    {
        //Reseteo de posici�n
        //===============================================================================
        // Calcula la distancia recorrida
        distanceTraveled += Mathf.Abs(rb.velocity.x) * Time.deltaTime;

        // Actualiza el tiempo desde el �ltimo reset
        timeSinceLastReset += Time.deltaTime;

        // Verifica si se debe realizar un reset
        if (distanceTraveled >= resetDistance || timeSinceLastReset >= resetDelay)
        {
            // Realiza el reset a la posici�n deseada
            ResetPosition();
        }

        //Gizmos
        //===============================================================================
        // Checa si est� en el suelo
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, radius, groundLayer);

        // Checa si est� chocando con una pared
        isTouchingWall = Physics2D.OverlapCircle(wallCheck.position, radius, wallLayer);


        //Salto
        //===============================================================================
        // Habilita el salto si est� en el suelo o tocando una pared
        canJump = isGrounded || isTouchingWall;

        WallSlide();
        WallJump();

        if (!isWallJumping)
        {
            Flip();
        }
        /*
        if (Input.GetKeyDown(KeyCode.Space) && canJump)
        {
            Jump();
        }


        if (Input.GetKeyDown(KeyCode.Space) || button != true)
        {
            if (isGrounded)
            {
                Jump(); // Salto normal si est� en el suelo
            }
            else if (jumpsPerformed < maxJumps)
            {
                // Segundo salto controlado si quedan saltos disponibles
                rb.velocity = new Vector2(rb.velocity.x, secondJumpForce);
                jumpsPerformed++;
            }
        } 
        */
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
        {
            jumpsPerformed = 0; // Restablecer los saltos cuando toca el suelo
            isJumping = false; // El jugador no est� saltando
        }

        if ((isGrounded || jumpsPerformed < maxJumps || isWallJumping || isWallSliding) && !isJumping)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpSpeed);
            jumpsPerformed++;
            isJumping = true; // El jugador est� saltando
        }
        else if (jumpsPerformed < maxJumps && !isGrounded && !isWallJumping && !isWallSliding)
        {
            // Realizar el segundo salto si se cumplen las condiciones
            rb.velocity = new Vector2(rb.velocity.x, secondJumpForce);
            jumpsPerformed++;
        }
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
            // Cambia la fuerza de salto solo para el wall jump
            jumpSpeed = wallJumpingPower.y;

            isWallJumping = true;
            wallJumpingDirection = -transform.localScale.x;
            wallJumpingCounter = wallJumpingTime;

            rb.velocity = new Vector2(wallJumpingPower.x * wallJumpingDirection, wallJumpingPower.y);

            if (transform.localScale.x != wallJumpingDirection)
            {
                isFacingright = !isFacingright;
                Vector3 localScale = transform.localScale;
                localScale.x *= -1f;
                transform.localScale = localScale;
            }

            Invoke(nameof(StopWallJumping), wallJumpingDuration);
        }
        else
        {
            wallJumpingCounter -= Time.deltaTime;
        }

        // Restablecer la l�gica del segundo salto
        if (isGrounded)
        {
            isWallJumping = false;
            wallJumpingCounter = wallJumpingTime;
        }
    }
    private void StopWallJumping()
    {
       isWallJumping = false;
        // Restaura la fuerza de salto original despu�s del wall jump
        jumpSpeed = originalJumpSpeed;
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

    private void ResetPosition()
    {
        if (isGrounded)
        {
            // Solo reinicia la posici�n en el eje X si est� en el suelo
            Vector3 newPosition = transform.position;
            newPosition.x = initialPosition.x; // Mant�n la misma posici�n en Y y Z
            transform.position = newPosition;
            
            // Reinicia la distancia recorrida y el tiempo desde el �ltimo reset
            distanceTraveled = 0f;
            timeSinceLastReset = 0f;
        }

    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.CompareTag("Obstacle"))
        {
            GameManager.Instance.ShowGameOverScreen();
            Time.timeScale = 0f;
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Friccion"))
        {
            // Aplica una fuerza de fricci�n al personaje
            rb.transform.Translate(Vector2.left * friction);
            frictionActive = true;
        }

        if (collision.gameObject.CompareTag("Freno"))
        {
            // Aplica una fuerza de fricci�n al personaje
            rb.AddForce(-rb.velocity.normalized * freno);
            //rb.velocity = new Vector2(wallJumpingPower.x * wallJumpingDirection, wallJumpingPower.y)*-freno;
            transform.Translate(Vector2.left * Time.deltaTime * freno);
            frenoActive = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Friccion"))
        {
            // Detiene la aplicaci�n de fricci�n cuando sale del objeto de fricci�n
            frictionActive = false;
        }
        
        if (collision.gameObject.CompareTag("Freno"))
        {
            // Detiene la aplicaci�n de fricci�n cuando sale del objeto de fricci�n
            frenoActive = false;
        }
    }

}