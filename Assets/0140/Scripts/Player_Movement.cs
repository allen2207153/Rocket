using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
//using static UnityEditorInternal.VersionControl.ListControl;

public class Player_Movement : MonoBehaviour
{
    [Header("Movement")]
    private float moveSpeed;  // Current movement speed
    private float desiredMoveSpeed;  // Desired movement speed
    private float lastDesiredMoveSpeed;  // Last desired movement speed
    private MovementState lastState;
    public float walkSpeed;  // Walking speed
    public float sprintSpeed;  // Sprinting speed
    public float slideSpeed;  // Sliding speed
    public float wallrunSpeed;  // Wall running speed
    public float climbSpeed;  // Climbing speed
    public float vaultSpeed;  // Vaulting speed
    public float airMinSpeed;  // Minimum air speed
    public float dashSpeed;
    public float dashSpeedChangeFactor;
    public float maxYSpeed;

    public float speedIncreaseMultiplier;  // Multiplier for speed increase
    public float slopeIncreaseMultiplier;  // Multiplier for slope speed increase

    public float groundDrag;  // Drag on the ground

    [Header("Jumping")]
    public float jumpForce;  // Force applied when jumping
    public float jumpCooldown;  // Time before next jump
    public float airMultiplier;  // Multiplier for air control
    bool readyToJump;  // Is the player ready to jump?

    [Header("Crouching")]
    public float crouchSpeed;  // Crouching speed
    public float crouchYScale;  // Scale of the player while crouching
    private float startYScale;  // Starting Y scale of the player

    [Header("Keybinds")]
    public KeyCode jumpKey = KeyCode.Space;  // Key to jump
    public KeyCode sprintKey = KeyCode.LeftShift;  // Key to sprint
    public KeyCode crouchKey = KeyCode.LeftControl;  // Key to crouch

    [Header("Ground Check")]
    public float playerHeight;  // Height of the player
    public LayerMask whatIsGround;  // Layer mask for ground detection
    public bool grounded;  // Is the player grounded?

    [Header("Slope Handling")]
    public float maxSlopeAngle;  // Maximum angle for slope movement
    private RaycastHit slopeHit;  // Information about the slope hit
    private bool exitingSlope;  // Are we exiting a slope?

    [Header("References")]
    public Climbing climbingScript;  // Reference to the climbing script
    //private ClimbingDone climbingScriptDone;

    public Transform orientation;  // Orientation of the player for movement directions

    float horizontalInput;  // Horizontal input for movement (e.g., A/D keys)
    float verticalInput;  // Vertical input for movement (e.g., W/S keys)

    Vector3 moveDirection;  // Direction the player is moving in

    Rigidbody rb;  // Reference to the player's Rigidbody

    public MovementState state;  // Current movement state
    public enum MovementState
    {
        freeze,  // Player is frozen
        unlimited,  // Player has unlimited movement
        walking,  // Player is walking
        sprinting,  // Player is sprinting
        wallrunning,  // Player is wall running
        climbing,  // Player is climbing
        vaulting,  // Player is vaulting
        crouching,  // Player is crouching
        sliding,  // Player is sliding
        dashing,
        air  // Player is in the air
    }

    public bool sliding;  // Is the player sliding?
    public bool crouching;  // Is the player crouching?
    public bool wallrunning;  // Is the player wall running?
    public bool climbing;  // Is the player climbing?
    public bool dashing;
    public bool vaulting;  // Is the player vaulting?

    public bool freeze;  // Is the player frozen?
    public bool unlimited;  // Does the player have unlimited movement?

    public bool restricted;  // Is the player movement restricted?

    public TextMeshProUGUI text_speed;  // Speed display text
    public TextMeshProUGUI text_mode;  // Current mode display text

    private void Start()
    {
        // Initialize the Rigidbody component
        //climbingScriptDone = GetComponent<ClimbingDone>();
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;  // Prevent rotation

        readyToJump = true;  // Player is ready to jump

        startYScale = transform.localScale.y;  // Store the starting Y scale for crouch
    }

    private void Update()
    {
        // ground check using a raycast to detect if the player is grounded
        grounded = Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.5f + 0.2f, whatIsGround);

        MyInput();  // Get player input
        SpeedControl();  // Control the player's speed
        StateHandler();  // Handle different movement states
        //TextStuff();  // Update the text display for speed and mode

        // Handle drag based on whether the player is grounded or in the air
        if (grounded)
            rb.drag = groundDrag;
        else
            rb.drag = 0;
    }

    private void FixedUpdate()
    {
        MovePlayer();  // Apply movement to the player
    }

    private void MyInput()
    {
        // Get horizontal and vertical input
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");

        // When to jump: player presses the jump key and is ready to jump
        if (Input.GetKey(jumpKey) && readyToJump && grounded)
        {
            readyToJump = false;  // Player is no longer ready to jump
            Jump();  // Perform the jump
            Invoke(nameof(ResetJump), jumpCooldown);  // Reset jump availability after cooldown
        }

        // Start crouch: when crouch key is pressed and no movement input
        if (Input.GetKeyDown(crouchKey) && horizontalInput == 0 && verticalInput == 0)
        {
            transform.localScale = new Vector3(transform.localScale.x, crouchYScale, transform.localScale.z);
            rb.AddForce(Vector3.down * 5f, ForceMode.Impulse);  // Add downward force

            crouching = true;  // Player is crouching
        }

        // Stop crouch: when crouch key is released
        if (Input.GetKeyUp(crouchKey))
        {
            transform.localScale = new Vector3(transform.localScale.x, startYScale, transform.localScale.z);  // Reset the player's Y scale

            crouching = false;  // Player is no longer crouching
        }
    }

    bool keepMomentum;
    private void StateHandler()
    {
        // Handle different movement states based on conditions
        if (freeze)
        {
            state = MovementState.freeze;  // Player is frozen
            rb.velocity = Vector3.zero;  // Stop movement
            desiredMoveSpeed = 0f;  // No movement speed
        }

        else if (unlimited)
        {
            state = MovementState.unlimited;  // Player has unlimited movement
            desiredMoveSpeed = 999f;  // Set a high speed
        }

        else if (vaulting)
        {
            state = MovementState.vaulting;  // Player is vaulting
            desiredMoveSpeed = vaultSpeed;  // Set vault speed
        }

        else if (dashing)
        {
            state = MovementState.dashing;
            desiredMoveSpeed = dashSpeed;
            speedChangeFactor = dashSpeedChangeFactor;
        }

        else if (climbing)
        {
            state = MovementState.climbing;  // Player is climbing
            desiredMoveSpeed = climbSpeed;  // Set climbing speed
        }

        else if (wallrunning)
        {
            state = MovementState.wallrunning;  // Player is wall running
            desiredMoveSpeed = wallrunSpeed;  // Set wallrun speed
        }

        else if (sliding)
        {
            state = MovementState.sliding;  // Player is sliding

            // Increase speed if on slope and moving downward
            if (OnSlope() && rb.velocity.y < 0.1f)
            {
                desiredMoveSpeed = slideSpeed;  // Set sliding speed
                keepMomentum = true;  // Keep momentum while sliding
            }

            else
                desiredMoveSpeed = sprintSpeed;  // Set sprinting speed
        }

        else if (crouching)
        {
            state = MovementState.crouching;  // Player is crouching
            desiredMoveSpeed = crouchSpeed;  // Set crouch speed
        }

        //else if (grounded && Input.GetKey(sprintKey))
        //{
        //    state = MovementState.sprinting;  // Player is sprinting
        //    desiredMoveSpeed = sprintSpeed;  // Set sprint speed
        //}

        else if (grounded)
        {
            state = MovementState.walking;  // Player is walking
            desiredMoveSpeed = walkSpeed;  // Set walk speed
        }

        else
        {
            state = MovementState.air;  // Player is in the air
            if (moveSpeed < airMinSpeed)
                desiredMoveSpeed = airMinSpeed;  // Set minimum air speed
        }

        bool desiredMoveSpeedHasChanged = desiredMoveSpeed != lastDesiredMoveSpeed;  // Check if desired speed has changed
        if (lastState == MovementState.dashing) keepMomentum = true;

        if (desiredMoveSpeedHasChanged)
        {
            if (keepMomentum)
            {
                StopAllCoroutines();  // Stop all running coroutines
                StartCoroutine(SmoothlyLerpMoveSpeed());  // Smoothly adjust speed
            }
            else
            {
                moveSpeed = desiredMoveSpeed;  // Directly set the movement speed
            }
        }

        lastDesiredMoveSpeed = desiredMoveSpeed;  // Update the last desired speed

        // Deactivate momentum maintenance if the speed difference is small
        if (Mathf.Abs(desiredMoveSpeed - moveSpeed) < 0.1f) keepMomentum = false;
    }

    private float speedChangeFactor;
    private IEnumerator SmoothlyLerpMoveSpeed()
    {
        // Smoothly interpolate the movement speed to the desired value
        float time = 0;
        float difference = Mathf.Abs(desiredMoveSpeed - moveSpeed);
        float startValue = moveSpeed;
        float boostFactor = speedChangeFactor;

        while (time < difference)
        {
            // Interpolate between current and desired speed
            moveSpeed = Mathf.Lerp(startValue, desiredMoveSpeed, time / difference);

            if (OnSlope())  // If on a slope, adjust the speed increase based on the slope's angle
            {
                float slopeAngle = Vector3.Angle(Vector3.up, slopeHit.normal);
                float slopeAngleIncrease = 1 + (slopeAngle / 90f);  // Increase speed based on slope angle

                time += Time.deltaTime * speedIncreaseMultiplier * slopeIncreaseMultiplier * slopeAngleIncrease;
            }
            else
                time += Time.deltaTime * speedIncreaseMultiplier;  // Apply speed multiplier when on flat ground

            yield return null;  // Wait for the next frame
        }

        moveSpeed = desiredMoveSpeed;  // Ensure the movement speed reaches the desired value
    }

    private void MovePlayer()
    {
        if (climbingScript.exitingWall) return;  // Prevent movement if exiting the wall during climbing
        //if (climbingScriptDone.exitingWall) return; // Prevent movement if exiting the wall during climbing (older script reference)
        //if (restricted) return;  // If movement is restricted, don't apply forces

        // Calculate the movement direction based on input and orientation
        moveDirection = orientation.forward * verticalInput + orientation.right * horizontalInput;

        // Handle movement on slopes
        if (OnSlope() && !exitingSlope)
        {
            rb.AddForce(GetSlopeMoveDirection(moveDirection) * moveSpeed * 20f, ForceMode.Force);

            if (rb.velocity.y > 0)
                rb.AddForce(Vector3.down * 80f, ForceMode.Force);  // Apply downward force to counter upward movement on slope
        }

        // Handle movement on the ground
        else if (grounded)
            rb.AddForce(moveDirection.normalized * moveSpeed * 10f, ForceMode.Force);

        // Handle movement in the air
        else if (!grounded)
            rb.AddForce(moveDirection.normalized * moveSpeed * 10f * airMultiplier, ForceMode.Force);  // Apply air control multiplier

        // Disable gravity when wall running to prevent unintended movement
        if (!wallrunning) rb.useGravity = !OnSlope();
    }

    private void SpeedControl()
    {
        // Limit speed on slopes
        if (OnSlope() && !exitingSlope)
        {
            if (rb.velocity.magnitude > moveSpeed)
                rb.velocity = rb.velocity.normalized * moveSpeed;  // Clamp speed to the moveSpeed value
        }

        // Limit speed on the ground or in the air
        else
        {
            Vector3 flatVel = new Vector3(rb.velocity.x, 0f, rb.velocity.z);  // Flatten velocity to ignore vertical speed

            if (flatVel.magnitude > moveSpeed)  // If the velocity exceeds moveSpeed
            {
                Vector3 limitedVel = flatVel.normalized * moveSpeed;  // Normalize and clamp the velocity
                rb.velocity = new Vector3(limitedVel.x, rb.velocity.y, limitedVel.z);  // Apply the limited velocity
            }
        }
    }

    private void Jump()
    {
        exitingSlope = true;  // Set the flag to indicate we're exiting the slope

        // Reset vertical velocity before applying jump force
        rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

        rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);  // Apply the jump force
    }

    private void ResetJump()
    {
        readyToJump = true;  // Player is ready to jump again

        exitingSlope = false;  // Reset the exiting slope flag
    }

    public bool OnSlope()
    {
        // Check if the player is on a slope by casting a ray downwards and checking the angle
        if (Physics.Raycast(transform.position, Vector3.down, out slopeHit, playerHeight * 0.5f + 0.3f))
        {
            float angle = Vector3.Angle(Vector3.up, slopeHit.normal);  // Calculate the slope angle
            return angle < maxSlopeAngle && angle != 0;  // Return true if the angle is less than the max slope angle
        }

        return false;  // Return false if not on a slope
    }

    public Vector3 GetSlopeMoveDirection(Vector3 direction)
    {
        // Return the direction adjusted for slope movement by projecting it onto the slope's plane
        return Vector3.ProjectOnPlane(direction, slopeHit.normal).normalized;
    }

    private void TextStuff()
    {
        // Update the speed text display
        Vector3 flatVel = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

        if (OnSlope())  // If on a slope, display the full speed (including vertical)
            text_speed.SetText("Speed: " + Round(rb.velocity.magnitude, 1) + " / " + Round(moveSpeed, 1));
        else  // Display only the horizontal speed
            text_speed.SetText("Speed: " + Round(flatVel.magnitude, 1) + " / " + Round(moveSpeed, 1));

        // Display the current movement state
        text_mode.SetText(state.ToString());
    }

    public static float Round(float value, int digits)
    {
        // Round the value to the specified number of digits
        float mult = Mathf.Pow(10.0f, (float)digits);
        return Mathf.Round(value * mult) / mult;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("GoalPlatform"))
        {
            Debug.Log("Player reached the goal platform!");
            TimerController timerController = FindObjectOfType<TimerController>();
            if (timerController != null)
            {
                timerController.OnReachGoal();
            }
        }
    }
}


