using System;
using System.Collections;

using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Input")]
    public KeyCode attackKey = KeyCode.Mouse0;
    public KeyCode jumpKey = KeyCode.Space;
    public string xMoveAxis = "Horizontal";
    public string yMoveAxis = "Vertical";

    [Header("Movement")]
    public float speed;
    public float jumpForce;
    public float groundedLeeway;
    public bool canJump;

    [Header("Dash")]
    public float dashingPower;
    public bool dashedOnce = false;
    public float dasingTime;
    public float dashingCooldown;
    private bool canDash = true;
    private bool isDashing;
    public float moveIntentionY;

    private Rigidbody2D rb = null;
    private float moveIntentionX = 0;

    private bool attempJump = false;
    private bool attempAttcak = false;

    private Animator anim;

    [Header("Particle")]
    public ParticleSystem dust;
    [SerializeField] private TrailRenderer trail;

    void Start()
    {
        anim = GetComponent<Animator>();

        if (GetComponent<Rigidbody2D>())
        {
            rb = GetComponent<Rigidbody2D>();
        }
    }

    private void OnCollisionStay2D(Collision2D other)
    {
        canJump = true;
        dashedOnce = false;
    }

    private void OnCollisionExit2D(Collision2D other)
    {
        canJump = false;

    }

    public void MoveCharacter(float x, float y)
    {
        if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
        {
            rb.AddForce(new Vector2(2 * x, 2 * y));
        }
        else
        {
            rb.AddForce(new Vector2(x, y));
        }
    }

    void Update()
    {
        if (isDashing)
        {
            return;
        }

        GetInput();
        HandleJump();
        HandleAttack();
        HandleDash();

        if (Input.GetKey(KeyCode.LeftShift) && canDash)
        {
            StartCoroutine(HandleDash());
            dust.Play();
        }

        /*if (Input.GetKey(KeyCode.LeftShift) && canDash)
        {
            StartCoroutine(HandleDash());
        }*/

        if (Input.GetKey(KeyCode.Space))
        {
            anim.SetBool("isJumping", true);
        }
        else{
            anim.SetBool("isJumping", false);
        }

        

        /*    if (Input.GetKey(KeyCode.A))
            {
                MoveCharacter(-1, 0);
            }
            else if (Input.GetKey(KeyCode.D))
            {
                MoveCharacter(1, 0);

            }
            else if (Input.GetKey(KeyCode.W))
            {
                MoveCharacter(0, canJump ? 1 : 0);

            }
            else if (Input.GetKey(KeyCode.S))
            {
                MoveCharacter(0, canJump ? 0 : -1);

            }
        }*/
    }

    void FixedUpdate()
    {
        if (isDashing)
        {
            return;
        }

        if (moveIntentionX == 0)
        {
            
            anim.SetBool("isRunning", false);
            
        }
        else
        {
            
            anim.SetBool("isRunning", true);
            
        }

        /*if(moveIntentionY == 0){
            anim.SetBool("isRunning", false);
        }
        else{
            anim.SetBool("isRunning", true);
        }*/

        HandleRun();
    }

    void OnDrawGizmosSelected()
    {
        Debug.DrawRay(transform.position, -Vector2.up * groundedLeeway, Color.green);
    }

    private void GetInput()
    {
        moveIntentionX = Input.GetAxis(xMoveAxis);
        moveIntentionY = Input.GetAxis(yMoveAxis);
        attempAttcak = Input.GetKeyDown(attackKey);
        attempJump = Input.GetKeyDown(jumpKey);
    }

    private void HandleRun()
    {
        if (moveIntentionX > 0 && transform.rotation.y == 0)
        {
            transform.rotation = Quaternion.Euler(0, 180f, 0);
            //dust.Play();
        }
        else if (moveIntentionX < 0 && transform.rotation.y != 0)
        {
            transform.rotation = Quaternion.Euler(0, 0, 0);
            //dust.Play();
        }

        rb.velocity = new Vector2(moveIntentionX * speed, rb.velocity.y);
    }

    private IEnumerator HandleDash()
    {
        canDash = false;
        isDashing = true;
        float originalGravity = rb.gravityScale;
        rb.gravityScale = 0f;
        rb.velocity = new Vector2(Mathf.RoundToInt(moveIntentionX) * dashingPower, (CheckGrounded() || !dashedOnce) ? jumpForce * Mathf.RoundToInt(moveIntentionY) : rb.velocity.y);
        trail.emitting = true;
        yield return new WaitForSeconds(dasingTime);
        trail.emitting = false;
        rb.gravityScale = originalGravity;
        isDashing = false;
        yield return new WaitForSeconds(dashingCooldown);
        canDash = true;
        dashedOnce = true;
    }

    private void HandleJump()
    {
        if (attempJump && CheckGrounded())
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
        }
    }

    private void HandleAttack()
    {

    }

    private bool CheckGrounded()
    {
        //return Physics2D.Raycast(transform.position, -Vector2.up, groundedLeeway);

        return canJump;
    }
}
