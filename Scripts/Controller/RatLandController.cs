using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RatLandController : MonoBehaviour
{
    public Transform CameraFollow;
    Rigidbody rb = null;
    SphereCollider SC = null;
    [SerializeField] RatLandInput input;
    Vector3 playerMoveInput = Vector3.zero;
    Vector3 playerLookInput = Vector3.zero;
    Vector3 previousPlayerLookInput= Vector3.zero;
    float cameraPitch = 0.0f;
    [SerializeField] float playerLookInputLT = 0.35f;

    //Movement
    [SerializeField] float mmMultiplyer = 28.0f;
    [SerializeField] float rotationSpeedMultiplyer = 180.0f;
    [SerializeField] float pitchSpeedMultiplyer = 180.0f;

    //Ground Check
    [SerializeField] bool playerGrounded = true;
    [SerializeField][Range(0.0f, 1.8f)] float groundCheckRadiusMult = 0.9f;
    [SerializeField][Range(-0.95f, 1.05f)] float groundCheckDistance = 0.05f;
    RaycastHit groundCheckHit = new RaycastHit();

    //Gravity
    [SerializeField] float gravityFallCurrent = -100.0f;
    [SerializeField] float gravityFallMin = -100.0f;
    [SerializeField] float gravityFallMax = -400.0f;
    [SerializeField][Range(-5.0f, -35.0f)] float gravityFallIncrement = -20.0f;
    [SerializeField] float gravityFallIncrementTime = 0.05f;
    [SerializeField] float playerFallTimer = 0.0f;

    //Jumping
    [SerializeField] float initialJumpForce = 1200.0f;
    [SerializeField] float continualJumpForceMultiplier = 0.1f;
    [SerializeField] float jumpTime = 0.175f;
    [SerializeField] float jumpTimeCounter = 0.0f;
    [SerializeField] float coyoteTime = 0.15f;
    [SerializeField] float coyoteTimeCounter = 0.0f;
    [SerializeField] float jumpBufferTime = 0.2f;
    [SerializeField] float jumpBufferTimeCounter = 0.0f;
    [SerializeField] bool playerIsJumping = false;
    [SerializeField] bool jumpWasPressedLastFrame = false;

    //  [SerializeField] float jumpReactionForce = 2000.0f;
    Collider standingCollider;
    private Animator animator;
    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
        SC = GetComponent<SphereCollider>();
    }
    private void FixedUpdate()
    {
        standingCollider = GetStandingCollider();
        mmMultiplyer = 28.0f;
        if (standingCollider != null)
        {
            if (standingCollider.CompareTag("Oil"))
            {
                mmMultiplyer = 80.0f;
            }
        }
        else
        {
            mmMultiplyer = 28.0f;
        }
        playerLookInput = GetLookInput();
        PlayerLook();
        PitchCamera();

        playerMoveInput = new Vector3(input.MoveInput.x, 0.0f, input.MoveInput.y);
        Vector3 ratAnimateCheck = CheckPlayerMovee();
        playerGrounded = PlayerGroundCheck();
        
        playerMoveInput.y = PlayerJump();
        playerMoveInput.y = PlayerGravity();
        playerMoveInput = PlayerMove();

       
        if (ratAnimateCheck != Vector3.zero)
        {
            animator.SetBool("IsMoving", true);
        }
        else
        {
            animator.SetBool("IsMoving", false);
        }
        rb.AddRelativeForce(playerMoveInput, ForceMode.Force);

    }

    private Vector3 GetLookInput()
    {
        previousPlayerLookInput = playerLookInput;
        playerLookInput = new Vector3(input.LookInput.x, (input.InvertMouseY ? - input.LookInput.y : input.LookInput.y), 0.0f);
        return Vector3.Lerp(previousPlayerLookInput, playerLookInput * Time.deltaTime, playerLookInputLT);
    }

    private void PlayerLook()
    {
        rb.rotation = Quaternion.Euler(0.0f, rb.rotation.eulerAngles.y + (playerLookInput.x * rotationSpeedMultiplyer), 0.0f);
    }

    private void PitchCamera()
    {
        Vector3 rotationValues = CameraFollow.rotation.eulerAngles;
        cameraPitch += playerLookInput.y * pitchSpeedMultiplyer;
        cameraPitch = Mathf.Clamp(cameraPitch, -89.9f, 89.9f);
        CameraFollow.rotation = Quaternion.Euler(cameraPitch, rotationValues.y, rotationValues.z);
    }

    private Vector3 PlayerMove()
    {
        Vector3 calculatedPlayerMovement = (new Vector3(playerMoveInput.x * mmMultiplyer * rb.mass,
            playerMoveInput.y * rb.mass,
            playerMoveInput.z * mmMultiplyer * rb.mass));
        return calculatedPlayerMovement;
    }

    private Vector3 CheckPlayerMovee()
    {
        Vector3 calculatedPlayerMovement = (new Vector3(playerMoveInput.x * mmMultiplyer * rb.mass,
            playerMoveInput.y,
            playerMoveInput.z * mmMultiplyer * rb.mass));
        return calculatedPlayerMovement;
    }
    private bool PlayerGroundCheck()
    {
        float sphereCastRadius = SC.radius * groundCheckRadiusMult;
        float sphereCastTravelDistance = sphereCastRadius + groundCheckDistance;
        return Physics.SphereCast(rb.position, sphereCastRadius, Vector3.down, out groundCheckHit, sphereCastTravelDistance);
    }
    private float PlayerGravity()
    {
        float gravity = playerMoveInput.y;
        if(playerGrounded)
        {
            gravityFallCurrent = gravityFallMin;

        }
        else
        {
            playerFallTimer -= Time.fixedDeltaTime;
            if(playerFallTimer < 0.0f)
            {
                if(gravityFallCurrent > gravityFallMax)
                {
                    gravityFallCurrent += gravityFallIncrement;
                }
                playerFallTimer = gravityFallIncrementTime;
               
            }
            gravity = gravityFallCurrent;
        }
        return gravity;
    }
    private float PlayerJump()
    {
        float calculatedJumpInput = playerMoveInput.y;

        SetJumpTimeCounter();
        SetCoyoteTimeCounter();
        SetJumpBufferTimeCounter();

        if (jumpBufferTimeCounter > 0.0f && !playerIsJumping && coyoteTimeCounter > 0.0f)
        {
            calculatedJumpInput = initialJumpForce;
            playerIsJumping = true;
            jumpBufferTimeCounter = 0.0f;
            coyoteTimeCounter = 0.0f;
        }
        else if (input.JumpIsPressed && playerIsJumping && !playerGrounded && jumpTimeCounter > 0.0f)
        {
            calculatedJumpInput = initialJumpForce * continualJumpForceMultiplier;
        }
        else if (playerIsJumping && playerGrounded)
        {
            playerIsJumping = false;
        }
        return calculatedJumpInput;
    }
    private void SetJumpTimeCounter()
    {
        if(playerIsJumping && !playerGrounded)
        {
            jumpTimeCounter -= Time.fixedDeltaTime;
        }
        else
        {
            jumpTimeCounter = jumpTime;
        }
    }
    private void SetCoyoteTimeCounter()
    {
        if (playerGrounded)
        {
            coyoteTimeCounter = coyoteTime;
        }
        else
        {
            coyoteTimeCounter -= Time.fixedDeltaTime;
        }
    }
    private void SetJumpBufferTimeCounter()
    {
        if(!jumpWasPressedLastFrame && input.JumpIsPressed)
        {
            jumpBufferTimeCounter = jumpBufferTime;
        }
        else if (jumpBufferTimeCounter > 0.0f)
        {
            jumpBufferTimeCounter -= Time.fixedDeltaTime;
        }
        jumpWasPressedLastFrame = input.JumpIsPressed;
    }
    Collider GetStandingCollider()
    {
        // Perform a raycast down from the player's position to detect the object they are standing on
        RaycastHit hit;
        if (Physics.Raycast(transform.position, Vector3.down, out hit, 1f))
        {
            return hit.collider;
        }
        return null;
    }
}
