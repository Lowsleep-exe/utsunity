using System;
using UnityEngine;
using UnityEngine.InputSystem;


public class Movement : MonoBehaviour
{
    public float speed = 5f;
    public float airSpeed = 2.5f;
    private PlayerControls playerControls;
    private Vector2 moveInput;
    private Rigidbody2D rb;
    public float jumpForce = 5f;

    public bool facingRight = true;

    #region GroundCheck

    public Vector2 boxSize;
    public LayerMask groundLayer;
    public LayerMask OreLayer;
    public float castDistance;

    
    public GameObject oreInRange;

    #endregion

    

    [SerializeField] private Camera mainCamera;
    private Vector2 pointerInput;
    private Vector2 lookInput;

    [SerializeField]
    public float rotationSpeed = 720f;

    public Transform Hand;

    public Vector2? targetDirection = null;

    public float charge;
    public bool isHolding;

    public ScoreManager scoremngr;

    public GameObject shovel;

    #region BodyParts Buffs/Debuffs

    public float SpeedMM = 0f;
    public float JumpMM = 0f;
    public float DamageMM = 0f;
    public float HealthMM = 0f;

    public AudioSource DigSound;

    



    #endregion





    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        
    }

    void Move()
    {
        // if (facingRight == false && moveInput.x > 0)
        // {
        //     Flip();
        // }
        // else if (facingRight == true && moveInput.x < 0)
        // {
        //     Flip();
        // }


        if (isGrounded() == false)
        {
            Vector2 movement = moveInput * (airSpeed + ((airSpeed / 100) * SpeedMM)) * Time.deltaTime;
            rb.linearVelocityX = movement.x;
        }
        else
        {
            Vector2 movement = moveInput * (speed + ((speed / 100) * SpeedMM)) * Time.deltaTime;
            rb.linearVelocityX = movement.x;
        }
        
    }

    private void HandleLook()
    {
        

        // --- Gamepad stick ---
        if (lookInput.sqrMagnitude > 0.01f)
        {
            targetDirection = lookInput.normalized;
        }
        else
        {
            // --- Mouse position ---
            Vector3 worldPoint = mainCamera.ScreenToWorldPoint(pointerInput);
            Vector2 direction2D = (Vector2)worldPoint - (Vector2)Hand.position;

            if (direction2D.sqrMagnitude > 0.001f)
                targetDirection = direction2D.normalized;
        }

        // --- Apply rotation ---
        if (targetDirection.HasValue)
        {
            float targetAngle = Mathf.Atan2(targetDirection.Value.y, targetDirection.Value.x) * Mathf.Rad2Deg;
            Quaternion targetRotation = Quaternion.AngleAxis(facingRight ? targetAngle : 180 - targetAngle, (facingRight ? 1 : -1) * Vector3.forward);

            Hand.rotation = Quaternion.RotateTowards(
                Hand.rotation,
                targetRotation,
                rotationSpeed * Time.deltaTime
            );
        }
    }



    void OnEnable()
    {
        if (playerControls == null)
        {
            playerControls = new PlayerControls();
            playerControls.Player.Move.performed += OnMove;
            playerControls.Player.Move.canceled += OnMove;
            playerControls.Player.Jump.performed += OnJump;
            playerControls.Player.Jump.canceled += OnJump;
            playerControls.Player.Dig.started += OnDig;
            playerControls.Player.Dig.performed += OnDig;
            playerControls.Player.Dig.canceled += OnDig;
            playerControls.Player.Point.performed += ctx => pointerInput = ctx.ReadValue<Vector2>();
            playerControls.Player.Point.canceled += ctx => pointerInput = Vector2.zero;
            playerControls.Player.Look.performed += ctx => lookInput = ctx.ReadValue<Vector2>();
            playerControls.Player.Look.canceled += ctx => lookInput = Vector2.zero;

            
        }
        playerControls.Player.Enable();
    }

    void OnDisable()
    {
        if (playerControls == null)
        {
            playerControls = new PlayerControls();
            playerControls.Player.Move.performed -= OnMove;
            playerControls.Player.Move.canceled -= OnMove;
            playerControls.Player.Jump.performed -= OnJump;
            playerControls.Player.Jump.canceled -= OnJump;
            playerControls.Player.Dig.started -= OnDig;
            playerControls.Player.Dig.performed -= OnDig;
            playerControls.Player.Dig.canceled -= OnDig;

            
        }
        playerControls.Player.Disable();
    }

    void Update()
    {
        HandleLook();
        //Move();
        //Debug.Log(isGrounded());
        //Debug.Log(speed + ((speed / 100) * SpeedMM));

        // if(Input.GetKeyDown(KeyCode.Backspace) && targetDirection.HasValue)
        // {
            
        // }

        // Debug.Log(targetDirection);

        if(isHolding == true)
        {
            charge += Time.deltaTime;
        }
        else
        {
            charge = 0;
        }

        charge = Mathf.Clamp(charge, 0f, 1.1f);

        
    }

    public void OnDig(InputAction.CallbackContext context)
    {

        if(context.started) isHolding = true;

        // if (context.performed)
        // {
            

        // }

        if( context.canceled)
        {
            if(isGrounded() && !isOre())
            {
                rb.linearVelocity = (Vector2)targetDirection * -(jumpForce + (charge * 10));
                DigSound.Play();
            }
            
            if(isOre())
            {
                scoremngr.AddScore(1);
                Debug.Log("+1");
                Destroy(oreInRange, 0.1f);
            }
    
            isHolding = false;
        }
    }
    
    public void Flip()
    {
        Vector3 currentScale = gameObject.transform.localScale;
        currentScale.x *= -1;
        gameObject.transform.localScale = currentScale;

        facingRight = !facingRight;
    }

    void OnMove(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();
    }

    
    private void OnLook(InputAction.CallbackContext context)
    {
        lookInput = context.ReadValue<Vector2>();
    }

    void OnJump(InputAction.CallbackContext context)
    {
        if (context.performed && isGrounded())
        {
            rb.linearVelocityY = jumpForce + ((jumpForce / 100) * JumpMM);
        }
        else if (context.canceled)
        {
            rb.linearVelocityY += -(jumpForce * 0.30f);

        }
    }

    public bool isGrounded()
    {
        RaycastHit2D ray = Physics2D.BoxCast(shovel.transform.position, boxSize, 0f, -shovel.transform.up, castDistance, groundLayer);
        if (ray.collider != null)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public bool isOre()
    {
        RaycastHit2D ray = Physics2D.BoxCast(shovel.transform.position, boxSize, 0f, -shovel.transform.up, castDistance, OreLayer);
        if (ray.collider != null)
        {
            oreInRange = ray.collider.gameObject;
            return true;
        }
        else
        {
            return false;
        }
    }
    
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(shovel.transform.position-shovel.transform.up * castDistance, boxSize);
    }
    
}