using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("การเคลื่อนไหวปกติ")]
    public float moveSpeed = 5f;
    public float jumpForce = 12f;
    private float moveInput;
    private bool isFacingRight = true;

    [Header("ระบบไต่กำแพง")]
    public float wallClimbSpeed = 3f;
    private bool isWallClimbing = false;
    public Transform wallCheck; //เช็คว่าอยู่ติดกำแพงมั้ย
    public LayerMask wallLayer;

    [Header("ระบบพุ่ง")]
    public float dashSpeed = 15f;
    public float dashTime = 0.2f;
    public float dashCooldown = 2f;
    private bool isDashing = false;
    private float nextDashTime = 0f;

    [Header("ระบบต่อสู้")]
    public bool isAttacking = false;
    public Transform attackPoint; //จุดโจมตี
    public float attackRange = 0.5f; //แอเรียการทำดาเมจ
    public LayerMask enemyLayer;
    public float attackDamage = 10f; //ความเสียหาย

    private Rigidbody2D rb;
    private bool isGrounded; //เช็คว่าอยู่บนพื้นมั้ย
    public Transform groundCheck;
    public LayerMask groundLayer;

    public bool isAuraBladeActive = false; // เช็คว่าบัฟออร่าดาบทำงานไหม

    private Animator anim;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        moveInput = Input.GetAxisRaw("Horizontal");

        // อนิเมชั่นเดิน
        anim.SetBool("isWalking", moveInput != 0 && isGrounded);

        //กระโดด
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            anim.SetTrigger("Jump");
        }

        //พุ่ง
        if (Input.GetKeyDown(KeyCode.LeftShift) && Time.time >= nextDashTime)
        {
            StartCoroutine(Dash());
        }

        //เช็กว่าตัวละครติดกำแพงมั้ย
        bool isTouchingWall = Physics2D.OverlapCircle(wallCheck.position, 0.1f, wallLayer);
        //ปีนกำแพง
        if (isTouchingWall && Input.GetKey(KeyCode.Space) && moveInput != 0)
        {
            isWallClimbing = true;
            rb.velocity = new Vector2(rb.velocity.x, wallClimbSpeed);
            anim.SetBool("isClimbing", true);
        }
        else
        {
            isWallClimbing = false;
            anim.SetBool("isClimbing", false);
        }

        if (isWallClimbing && !isTouchingWall)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
        }

        //โจมตี
        if (Input.GetMouseButtonDown(0) && !isAttacking)
        {
            Attack();
            anim.SetTrigger("Attack");
        }

        if (moveInput > 0 && !isFacingRight)
            Flip();
        else if (moveInput < 0 && isFacingRight)
            Flip();
    }

    void FixedUpdate()
    {
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, 0.1f, groundLayer);

        if (!isDashing && !isWallClimbing)
        {
            rb.velocity = new Vector2(moveInput * moveSpeed, rb.velocity.y);
        }
    }

    private IEnumerator Dash()
    {
        isDashing = true;
        rb.velocity = new Vector2(transform.localScale.x * dashSpeed, 0f);
        yield return new WaitForSeconds(dashTime);
        isDashing = false;
        nextDashTime = Time.time + dashCooldown;
    }

    void Flip()
    {
        isFacingRight = !isFacingRight;
        Vector3 scaler = transform.localScale;
        scaler.x *= -1;
        transform.localScale = scaler;
    }

    void Attack()
    {
        isAttacking = true;
        anim.SetTrigger("Attack");
        Debug.Log("Player Attack!");

        //ตรวจศัตรูที่อยู่ในระยะ
        Collider2D[] enemiesToDamage = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayer);

        foreach (Collider2D enemy in enemiesToDamage)
        {
            //สร้างความเสียหาย
            enemy.GetComponent<Enemy>().TakeDamage(attackDamage);
        }

        Invoke("ResetAttack", 0.5f);
    }
    void OnTriggerEnter(Collider other)
    {
        if (isAuraBladeActive && ((1 << other.gameObject.layer) & enemyLayer) != 0)
        {
            // ทำให้ศัตรูถอยหลังเล็กน้อย
            Rigidbody enemyRb = other.GetComponent<Rigidbody>();
            if (enemyRb)
            {
                enemyRb.AddForce(transform.forward * 5f, ForceMode.Impulse);
            }
        }
    }

    void ResetAttack()
    {
        isAttacking = false;
    }

    void OnDrawGizmosSelected()
    {
        if (attackPoint == null)
            return;

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }
}