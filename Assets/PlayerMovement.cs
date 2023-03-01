using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour {
    [SerializeField] float speed, friction = 0.025f, maxSpeed;
    Vector2 inputDir;
    Rigidbody2D rb;

    [Header("Jumping")]
    [SerializeField] Vector2 groundCheckOffset;
    [SerializeField] float groundCheckRadius, jumpForce, jumpInputWindow, maxJumpSpeed, maxJumpUpTime;
    [SerializeField] string groundTag = "Ground";
    float timeSinceJumpPressed = Mathf.Infinity, jumpUpTime, timeSinceDashed;
    bool jumpButtonDown, jumping, grounded;
    public bool stopped;

    [Header("Dashing")]
    [SerializeField] float dashSpeed;
    [SerializeField] float dashTime, dashCooldown;
    [SerializeField] public bool canDash;
    bool dashing;

    [Header("sfx")]
    [SerializeField] AudioSource source;
    [SerializeField] int footstep, land, jump;
    

    public void OnMove(InputAction.CallbackContext ctx) => inputDir = ctx.ReadValue<Vector2>();

    public void OnJump(InputAction.CallbackContext ctx)
    {
        if (ctx.started) { timeSinceJumpPressed = 0; jumpButtonDown = true; }
        if (ctx.canceled) { timeSinceJumpPressed = Mathf.Infinity; jumpButtonDown = false; jumping = false;}
    }

    public void PressDash(InputAction.CallbackContext ctx)
    {
        if (!canDash || dashing || timeSinceDashed < dashCooldown || !ctx.started || !GameManager.instance.fighting) return;
        Dash();
    }
    void Dash()
    {
        timeSinceDashed = 0;
        StopAllCoroutines();
        StartCoroutine(DashCoroutine());
    }

    IEnumerator DashCoroutine()
    {
        float time = 0;
        dashing = true;
        var dir = transform.right;
        if (Mathf.Abs(rb.velocity.x) > 0.01f) dir = inputDir;
        dir.y = 0;

        while (time < dashTime) {
            rb.velocity = dir.normalized * dashSpeed;    
            time += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }

        dashing = false;
    }

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        Camera.main.GetComponent<CameraController>().players.Add(transform);
    }

    private void Update()
    {
        timeSinceDashed += Time.deltaTime;
        timeSinceJumpPressed += Time.deltaTime;
        if (jumping) jumpUpTime += Time.deltaTime;
        GetComponent<PlayerFighting>().enabled = !stopped;
        
        if (stopped || dashing) return;

        MoveLeftRight();
        DoJump();   
    }

    void MoveLeftRight()
    {
        inputDir.y = 0;
        if (Mathf.Abs(inputDir.x) > 0.001f && Mathf.Abs(rb.velocity.x) < maxSpeed) rb.velocity += (inputDir * speed * Time.deltaTime);
        else rb.velocity = Vector2.Lerp(rb.velocity, new Vector2(0, rb.velocity.y), friction);

        transform.eulerAngles = FaceDirection();
        
        if (Mathf.Abs(rb.velocity.x) > 0.1f && grounded) AudioManager.instance.PlaySoundNoInturrupt(1, source);
        else AudioManager.instance.StopSound(footstep, source);
    }

    Vector3 FaceDirection()
    {
        var moveRot = Mathf.Abs(rb.velocity.x) > 0.01f ? Vector3.up * (rb.velocity.x > 0 ? 0 : 180) : transform.eulerAngles;
        var otherPlayer = GameManager.instance.otherPlayerPosition(GetComponent<PlayerStats>());
        if (otherPlayer == transform.position) return moveRot;

        return Vector3.up * (otherPlayer.x < transform.position.x ? 180 : 0);
    }

    void DoJump()
    {
        grounded = IsGrounded();
        if (grounded) jumping = false;

        if (!jumpButtonDown) return;
        StartJump();
        ContinueJump();
    }


    void ContinueJump()
    {
        if (!jumping || jumpUpTime >= maxJumpUpTime) return;

        var vel2 = rb.velocity + (Vector2.up * jumpForce);
        vel2.y = Mathf.Min(vel2.y, maxJumpSpeed);
        rb.velocity = vel2;
    }

    void StartJump()
    {
        if (!grounded) { return; }

        jumpUpTime = 0;
        jumping = true;
        timeSinceJumpPressed = Mathf.Infinity;
        var vel1 = rb.velocity + (Vector2.up * jumpForce);
        vel1.y = Mathf.Min(vel1.y, maxJumpSpeed);
        rb.velocity = vel1;
    }

    bool IsGrounded()
    {
        var colliders = Physics2D.OverlapCircleAll((Vector2)transform.position + groundCheckOffset, groundCheckRadius);
        foreach(var c in colliders) if (c.CompareTag(groundTag)) return true;
        return false;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere((Vector2)transform.position + groundCheckOffset, groundCheckRadius);
    }

}
