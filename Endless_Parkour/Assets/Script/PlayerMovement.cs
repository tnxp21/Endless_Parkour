using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEditor;
using UnityEditor.Animations;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Speed info")]
    [SerializeField] private float maxSpeed;
    [SerializeField] private float speedMultiplier;
    [Space]
    [SerializeField] private float milestoneIncreaser;
    float speedMilestone;

    [Header("Move info")]
    [SerializeField] float runSpeed = 1.0f;
    [SerializeField] float jumpForce = 10.0f;
    bool baseJump = true;
    [SerializeField] float doubleJumpForce = 8.0f;
    Rigidbody2D rb;


    [Header("Slide info")]
    [SerializeField] float slideSpeed;
    [SerializeField] float slideTime;
    [SerializeField] float slideCoolDownTime = 0.0f;
    float slideTimeCounter = 0.0f;
    bool isSliding;


    [Header("Collision info")]
    [SerializeField] float checkDistanceToGround = 1.5f;
    [SerializeField] float checkDistanceToCeiling = 1.5f;
    public LayerMask groundMask;
    bool runBegin = false;
    bool isOnGround = false;
    bool ceilingDetected = false;
    bool wallDetected = false;
    [SerializeField] Transform wallCheckBox;
    [SerializeField] Vector2 wallCheckBoxSize;
    int JumpCount = 0;
    bool canDoubleJump = false;
    [SerializeField] int JumpBoostedCount = 0;

    [Header("Ledge info")]
    [SerializeField] Vector2 offset1;
    [SerializeField] Vector2 offset2;
    Vector2 climbBeginPosition;
    Vector2 climbOverPosition;
    bool canGrabLedge = true;
    bool canClimb;
    [HideInInspector] public bool ledgeDetected;

    [Header("Animation")]
    [SerializeField] Animator anim;

    //[SerializeField] bool isRunning = false;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        speedMilestone = milestoneIncreaser;
    }

    // Update is called once per frame
    void Update()
    {
        CheckCollision();
        GetInput();
        if (runBegin) Movement();
        AnimatorController();
        Timing();
        CheckForSliding();
        CheckForLedgeToClimb();
        SpeedController();
        //Debug.Log(ledgeDetected);
    }

    private void Timing()
    {
        slideTimeCounter -= Time.deltaTime;
    }

    private void SpeedController()
    {
        if (runSpeed == maxSpeed) return;
        if (transform.position.x > speedMilestone)
        {
            speedMilestone += milestoneIncreaser;
            runSpeed *= speedMultiplier;
            milestoneIncreaser *= speedMultiplier;
            runSpeed = runSpeed > maxSpeed ? maxSpeed : runSpeed;
        }
    }

    void CheckForSliding()
    {
        if (slideTimeCounter < 0 && isSliding && !ceilingDetected)
        {
            slideTimeCounter = slideCoolDownTime;
            isSliding = false;
            Debug.Log("Cooldown: " + slideTimeCounter);
        }
    }

    void CheckForLedgeToClimb()
    {
        if (ledgeDetected && canGrabLedge)
        {
            canGrabLedge = false;
            Vector2 ledgePosition = GetComponentInChildren<LegdeDetection>().transform.position;
            climbBeginPosition = ledgePosition + offset1;
            climbOverPosition = ledgePosition + offset2;
            canClimb = true;
        }
        if (canClimb)
            transform.position = climbBeginPosition;
    }

    void LedgeClimbOver()
    {
        canClimb = false;
        transform.position = climbOverPosition;
        Invoke("AllowLedgeGrab", .5f);
    }

    void AllowLedgeGrab() => canGrabLedge = true;

    private void Movement()
    {
        if (wallDetected) return;
        if (isSliding)

            rb.velocity = new Vector2(slideSpeed, rb.velocity.y);
        else
            rb.velocity = new Vector2(runSpeed, rb.velocity.y);
    }

    void GetInput()
    {
        if (Input.GetKeyDown(KeyCode.X))
        {
            runBegin = true;
        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            JumpingButton();
        }
        if (Input.GetKeyDown(KeyCode.LeftShift))
            SlidingButton();
    }

    //SlidingButton controller
    private void SlidingButton()
    {
        Debug.Log(slideTimeCounter);
        if (rb.velocity.x != 0 && isOnGround && slideTimeCounter <= 0)
        {
            isSliding = true;
            slideTimeCounter = slideTime;
        }
    }

    //JumpingButtoning action controller
    private void JumpingButton()
    {

        if (ceilingDetected) return;

        if (isOnGround)               //check to choose Jump force for player
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
        }
        else if (canDoubleJump)
        {
            canDoubleJump = false;
            rb.velocity = new Vector2(rb.velocity.x, doubleJumpForce);
        }
    }
    void AnimatorController()
    {
        anim.SetFloat("xVelocity", rb.velocity.x);
        anim.SetFloat("yVelocity", rb.velocity.y);

        anim.SetBool("canDoubleJump", canDoubleJump);
        anim.SetBool("isOnGround", isOnGround);
        anim.SetBool("isSliding", isSliding);
        anim.SetBool("canClimb", canClimb);
    }

    void CheckCollision()
    {
        isOnGround = Physics2D.Raycast(transform.position, Vector2.down, checkDistanceToGround, groundMask);
        if (isOnGround) canDoubleJump = true;
        ceilingDetected = Physics2D.Raycast(transform.position, Vector2.up, checkDistanceToGround, groundMask);
        wallDetected = Physics2D.BoxCast(wallCheckBox.position, wallCheckBoxSize, 0, Vector2.zero, 0, groundMask);

    }

    void OnDrawGizmos()
    {
        Gizmos.DrawLine(transform.position, new Vector2(transform.position.x, transform.position.y - checkDistanceToGround));
        Gizmos.DrawLine(transform.position, new Vector2(transform.position.x, transform.position.y + checkDistanceToCeiling));
        Gizmos.DrawWireCube(wallCheckBox.position, wallCheckBoxSize);
    }
}
