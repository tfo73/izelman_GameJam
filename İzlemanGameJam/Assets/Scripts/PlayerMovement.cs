using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Input")]
    public KeyCode attackKey = KeyCode.Mouse0;
    public KeyCode jumpKey = KeyCode.Space;
    public string xMoveAxis = "Horizontal";

    [Header("Movement")]
    public float speed;
    public float jumpForce;
    public float groundedLeeway;

    [Header("Dash")]    
    public float dashingPower;
    public float dasingTime;
    public float dashingCooldown;
    private bool canDash = true;
    private bool isDashing;

    private Rigidbody2D rb = null;
    private float moveIntentionX = 0;
    private bool attempJump = false;
    private bool attempAttcak = false;

    //[SerializeField] private TrailRenderer tr;

    void Start()
    {
        if(GetComponent<Rigidbody2D>()){
            rb = GetComponent<Rigidbody2D>();
        }
    }

    void Update()
    {
        if(isDashing){
            return;
        }

        GetInput();
        HandleJump();
        HandleAttack();
        
        if(Input.GetKey(KeyCode.LeftShift) && canDash){
            if(Input.GetKey(KeyCode.A)){
                StartCoroutine(DashRight());
            }
            else if(Input.GetKey(KeyCode.D)){
                StartCoroutine(DashLeft());
            } 
            else if(Input.GetKey(KeyCode.W)){
                StartCoroutine(DashUp());
            } 
            else if(Input.GetKey(KeyCode.S)){
                StartCoroutine(DashDown());
            } 
        }
    }

    void FixedUpdate(){
        if(isDashing){
            return;
        }

        HandleRun();
    }

    void OnDrawGizmosSelected(){
        Debug.DrawRay(transform.position, -Vector2.up * groundedLeeway, Color.green);
    }

    private void GetInput(){
        moveIntentionX = Input.GetAxis(xMoveAxis);
        attempAttcak = Input.GetKeyDown(attackKey);
        attempJump = Input.GetKeyDown(jumpKey);
    }

    private void HandleRun(){
        if(moveIntentionX > 0 && transform.rotation.y == 0){
            transform.rotation = Quaternion.Euler(0, 180f, 0);
        }
        else if(moveIntentionX < 0 && transform.rotation.y != 0){
            transform.rotation = Quaternion.Euler(0, 0, 0);
        }

        rb.velocity = new Vector2(moveIntentionX * speed, rb.velocity.y);
    }

    private void HandleJump(){
        if(attempJump && CheckGrounded()){
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
        }
    }

    private void HandleAttack(){

    }

    private bool CheckGrounded(){
        return Physics2D.Raycast(transform.position, -Vector2.up, groundedLeeway);
    }

    private IEnumerator DashRight(){
        canDash = false;
        isDashing = true;
        float originalGravity = rb.gravityScale;
        rb.gravityScale = 0f;
        rb.velocity = new Vector2(-transform.localScale.x * dashingPower, 0f);
        //tr.emitting = true;
        yield return new WaitForSeconds(dasingTime);
        //tr.emitting = false;
        rb.gravityScale = originalGravity;
        isDashing = false;
        yield return new WaitForSeconds(dashingCooldown);
        canDash = true;
    }

    private IEnumerator DashLeft(){
        canDash = false;
        isDashing = true;
        float originalGravity = rb.gravityScale;
        rb.gravityScale = 0f;
        rb.velocity = new Vector2(transform.localScale.x * dashingPower, 0f);
        //tr.emitting = true;
        yield return new WaitForSeconds(dasingTime);
        //tr.emitting = false;
        rb.gravityScale = originalGravity;
        isDashing = false;
        yield return new WaitForSeconds(dashingCooldown);
        canDash = true;
    }

    private IEnumerator DashUp(){
        canDash = false;
        isDashing = true;
        float originalGravity = rb.gravityScale;
        rb.gravityScale = 4.5f;
        rb.velocity = new Vector2(0f, transform.localScale.x * dashingPower);
        //tr.emitting = true;
        yield return new WaitForSeconds(dasingTime);
        //tr.emitting = false;
        rb.gravityScale = originalGravity;
        isDashing = false;
        yield return new WaitForSeconds(dashingCooldown);
        canDash = true;
    }
    private IEnumerator DashDown(){
        canDash = false;
        isDashing = true;
        float originalGravity = rb.gravityScale;
        rb.gravityScale = 0f;
        rb.velocity = new Vector2(0f, -transform.localScale.x * dashingPower);
        //tr.emitting = true;
        yield return new WaitForSeconds(dasingTime);
        //tr.emitting = false;
        rb.gravityScale = originalGravity;
        isDashing = false;
        yield return new WaitForSeconds(dashingCooldown);
        canDash = true;
    }
}
