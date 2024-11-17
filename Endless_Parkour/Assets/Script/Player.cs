using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Xml.Serialization;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class Player : MonoBehaviour
{
    Rigidbody2D rb;
    SpriteRenderer sr;
    bool isDead;
    [HideInInspector] public bool extraLife;

    [Header("VFX")]
    [SerializeField] ParticleSystem dustVFX;
    [SerializeField] ParticleSystem bloodSplashVFX;

    [Header("Speed info")]
    [SerializeField] private float maxSpeed;
    [SerializeField] private float speedToSurvive = 16f;
    [SerializeField] private float speedMultiplier;
    float defaultSpeed;
    [Space]
    [SerializeField] private float milestoneIncreaser;
    float defaultMilestoneIncrease;
    float speedMilestone;


    [Header("Move info")]
    [SerializeField] float runSpeed = 1.0f;
    [SerializeField] float jumpForce = 10.0f;
    [SerializeField] float doubleJumpForce = 8.0f;
    bool canHardLand = false;

    [Header("Knock back info")]
    [SerializeField] Vector2 knockBackDir;
    bool isKnocked;
    bool canBeKnock = true;

    [Header("Slide info")]
    [SerializeField] float slideSpeed;
    [SerializeField] float slideTime = 2.0f;
    [SerializeField] float slideCoolDownTime = 0.0f;
    float slideTimeCounter = 0.0f;
    bool isSliding;


    [Header("Collision info")]
    [SerializeField] float checkDistanceToGround = 1.5f;
    [SerializeField] float checkDistanceToCeiling = 1.5f;
    public LayerMask groundMask;
    [HideInInspector] public bool runBegin = false;
    bool isOnGround = false;
    bool ceilingDetected = false;
    bool wallDetected = false;
    [SerializeField] Transform wallCheckBox;
    [SerializeField] Vector2 wallCheckBoxSize;
    bool canDoubleJump = false;

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
        sr = GetComponent<SpriteRenderer>();
        speedMilestone = milestoneIncreaser;
        defaultSpeed = runSpeed;
        defaultMilestoneIncrease = milestoneIncreaser;
    }

    // Update is called once per frame
    void Update()
    {
        if (!runBegin) return;
        CheckCollision();
        AnimatorController();
        Timing();
        extraLifeInfo();

        if (isDead) return;
        if (isKnocked) return;

        GetInput();
        LandInfo();
        if (runBegin) MovementSetup();
        CheckForSlidingCancel();
        CheckForLedgeToClimb();
        SpeedController();
    }

    void LandInfo()
    {
        if (isDead) return;
        if (rb.velocity.y < -5 && !isOnGround)
        {
            canHardLand = true; 
        }
        if (canHardLand && isOnGround)
        {
            dustVFX.Play();
            canHardLand = false;
        }
    }
    void extraLifeInfo()
    {
        extraLife = runSpeed >= speedToSurvive;
    }

    private void Timing()
    {
        if (slideTimeCounter>0) slideTimeCounter -= Time.deltaTime;
        else slideTimeCounter = 0.0f;
    }

    public float GetSlideTimeCounterPercent() => isSliding ? 1 : Mathf.Round(slideTimeCounter / slideCoolDownTime * 100f) * 0.01f;

    public void Damage()
    {
        if (extraLife)
        {
            KnockBack();
        }
        else if (canBeKnock) Die();
    }

    public void SetIsDead(bool dead) => isDead = dead;
    public bool GetIsDead() => isDead;

    private void Die()
    {
        if (isDead) return;     // You cannot die twice right?? :>>
        isDead = true;
        rb.velocity = knockBackDir;
        anim.SetBool("isDead", true);
        bloodSplashVFX.Play(); 
        Time.timeScale = 0.3f;
        AudioManager.instance.PlaySFX(4);
        AudioManager.instance.StopAllBGM();
    }

    IEnumerator EndOfDeathAnim()
    {
        yield return new WaitForSeconds(.25f);
        rb.velocity = Vector2.zero;
        yield return new WaitForSeconds(.3f);
        GameManager.instance.GameEnded();
    }

    IEnumerator Invincibility()
    {

        canBeKnock = false;
        Color originalColor = sr.color;
        Color darkerColor = new Color(sr.color.r, sr.color.g, sr.color.b, 0.5f);

        float i = 0.1f;
        int fastBlinkTimes = 10; //number of fast bink after knocked before it gets slower
        bool isdarker = true; //to know what color will the sprite change in this interval
        while (i < 0.8f)
        {
            if (isdarker) sr.color = darkerColor;
            else sr.color = originalColor;
            isdarker = !isdarker;
            yield return new WaitForSeconds(i);
            if (fastBlinkTimes > 0) fastBlinkTimes--;
            else i += 0.1f;
        }
        sr.color = originalColor; //make sure the color gets back to it's original one
        canBeKnock = true;
    }

    #region Knockback
    void KnockBack()
    {
        if (!canBeKnock) return;
        StartCoroutine(Invincibility());
        isKnocked = true;
        rb.velocity = knockBackDir;
    }

    void CancelKnockBack()
    {
        isKnocked = false;
        SpeedReset();
    }

    #endregion

    #region Speed Control
    void SpeedReset()
    {
        runSpeed = defaultSpeed;
        milestoneIncreaser = defaultMilestoneIncrease;
    }

    private void SpeedController()
    {
        if (transform.position.x > speedMilestone)
        {
            speedMilestone += milestoneIncreaser;
            milestoneIncreaser *= speedMultiplier;
            if (runSpeed == maxSpeed) return;
            runSpeed *= speedMultiplier;
            runSpeed = runSpeed > maxSpeed ? maxSpeed : runSpeed;
        }
    }
    #endregion

    void CheckForSlidingCancel()
    {
        if (slideTimeCounter <= 0 && isSliding && !ceilingDetected)
        {
            slideTimeCounter = slideCoolDownTime;
            isSliding = false;
        }
    }

    #region Ledge Climb
    void CheckForLedgeToClimb()
    {
        if (ledgeDetected && canGrabLedge)
        {
            canGrabLedge = false;
            rb.gravityScale = 0;
            Vector2 ledgePosition = GetComponentInChildren<LegdeDetection>().transform.position;
            climbBeginPosition = ledgePosition + offset1;
            climbOverPosition = ledgePosition + offset2;
            canClimb = true;
        }
        if (canClimb)
            transform.position = climbBeginPosition;
    }

    void AllowLedgeGrab() => canGrabLedge = true;
    #endregion

    private void MovementSetup()
    {
        if (wallDetected) return;
        if (isSliding)
            rb.velocity = new Vector2(slideSpeed, rb.velocity.y);
        else
            rb.velocity = new Vector2(runSpeed, rb.velocity.y);
    }

    #region Inputs
    void GetInput()
    {

        // if (Input.GetKeyDown(KeyCode.KeypadEnter))
        // {
        //     runBegin = true;
        // }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            JumpingButton();
        }
        if (Input.GetKeyDown(KeyCode.LeftShift))
            SlidingButton();
    }

    //SlidingButton controller
    public void SlidingButton()
    {
        if (rb.velocity.x != 0 && isOnGround && slideTimeCounter <= 0)
        {
            isSliding = true;
            slideTimeCounter = slideTime;
            dustVFX.Play();
        }
    }

    //JumpingButtoning action controller
    public void JumpingButton()
    {

        if (ceilingDetected) return;

        RollAnimFinished();

        if (isOnGround)               //check to choose Jump force for player
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
        }
        else if (canDoubleJump)
        {
            canDoubleJump = false;
            rb.velocity = new Vector2(rb.velocity.x, doubleJumpForce);
        }
        else return;
        dustVFX.Play();
        AudioManager.instance.PlaySFX(UnityEngine.Random.Range(1, 2));
    }
    #endregion

    #region Animation
    void AnimatorController()
    {
        anim.SetFloat("xVelocity", rb.velocity.x);
        anim.SetFloat("yVelocity", rb.velocity.y);

        anim.SetBool("canDoubleJump", canDoubleJump);
        anim.SetBool("isOnGround", isOnGround);
        anim.SetBool("isSliding", isSliding);
        anim.SetBool("canClimb", canClimb);

        anim.SetBool("isKnocked", isKnocked);

        if (rb.velocity.y < -10)
            anim.SetBool("canRoll", true);
    }

    void RollAnimFinished() => anim.SetBool("canRoll", false);
    void LedgeClimbOver()
    {
        canClimb = false;
        rb.gravityScale = 5;
        transform.position = climbOverPosition;
        Invoke("AllowLedgeGrab", .5f);
    }

    #endregion

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
