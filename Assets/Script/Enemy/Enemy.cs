using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [Header("Enemy Setting")]
    public float detectionRange = 5f; // ระยะที่ศัตรูมองเห็นผู้เล่น
    public float moveSpeed = 3f;
    public float attackRange = 2f;
    public float attackDamage = 10f;
    public float health = 100f;
    public float attackCooldown = 1f;

    private bool isAttacking = false;
    private Transform player;
    private float nextAttackTime = 0f;

    public GameObject dropItemPrefab; // ไอเท็มที่ดร็อป เผื่อจะทำ
    public Transform dropPoint; // จุดที่ไอเท็มจะตก เผื่อจะทำ

    [Header("Patrol Settings")]
    public float patrolDistance = 3f;
    private Vector2 startPos;
    private int patrolDirection = 1; // 1 = เดินไปขวา, -1 = เดินไปซ้าย

    private Animator animator;
    private bool isDead = false; 

    void Start()
    {
        animator = GetComponent<Animator>();
        player = GameObject.FindGameObjectWithTag("Player").transform; // หาผู้เล่น
        startPos = transform.position; // บันทึกตำแหน่งเริ่มต้น
    }

    void Update()
    {
        float distanceToPlayer = Vector2.Distance(transform.position, player.position);

        if (distanceToPlayer <= attackRange && Time.time >= nextAttackTime)
        {
            AttackPlayer();
        }
        else if (distanceToPlayer <= detectionRange)
        {
            MoveTowardsPlayer();
        }
        else // ถ้าไม่อยู่ในระยะโจมตี ให้เดินไปมา
        {
            Patrol();
        }

        if (!isDead && health <= 0)
        {
            Die();
        }
    }

    void MoveTowardsPlayer()
    {
        animator.SetBool("isWalking", true);

        Vector2 direction = (player.position - transform.position).normalized;
        transform.position = Vector2.MoveTowards(transform.position, player.position, moveSpeed * Time.deltaTime);

        // หันหน้าเดินเข้าหาผู้เล่น
        if (direction.x > 0)
        {
            Flip(true);
        }
        else if (direction.x < 0)
        {
            Flip(false);
        }
    }

    void Patrol()
    {
        animator.SetBool("isWalking", true);

        transform.Translate(Vector2.right * patrolDirection * moveSpeed * Time.deltaTime); // เดินไปตามทิศทาง

        // ถ้าเดินไปไกลเกินระยะที่กำหนด ให้เปลี่ยนทิศทาง
        if (Mathf.Abs(transform.position.x - startPos.x) >= patrolDistance)
        {
            patrolDirection *= -1; // เปลี่ยนทิศทาง
            Flip(patrolDirection > 0);
        }
    }

    void AttackPlayer()
    {
        if (!isAttacking)
        {
            isAttacking = true;
            animator.SetTrigger("attack");
            nextAttackTime = Time.time + attackCooldown;

            player.GetComponent<PlayerHealth>().TakeDamage(attackDamage);

            Invoke("ResetAttack", 0.5f);
        }
    }

    void ResetAttack()
    {
        animator.SetBool("isWalking", false);
        isAttacking = false;
    }

    public void DropItem() // เผื่อทำเรื่องดร็อป
    {
        if (dropItemPrefab != null)
        {
            Instantiate(dropItemPrefab, dropPoint.position, Quaternion.identity);
        }
    }

    public void TakeDamage(float damage)
    {
        health -= damage;
        Debug.Log("Enemy HP: " + health);

        if (health <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        isDead = true;
        animator.SetTrigger("die");

        GetComponent<Collider2D>().enabled = false;

        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.velocity = Vector2.zero;
            rb.simulated = false;
        }

        this.enabled = false;
        Invoke(nameof(HandleDeath), 1.5f);

        //QuestTracker.Instance.EnemyKilled(gameObject.tag); // แจ้งให้ QuestTracker รู้ว่าศัตรูตายแล้ว
        //Destroy(gameObject);
        //DropItem(); // เผื่อจะทำดร็อปไอเท็ม
    }

    void HandleDeath()
    {
        QuestTracker.Instance.EnemyKilled(gameObject.tag);
        DropItem();
        Destroy(gameObject);
    }

    void Flip(bool facingLeft)
    {
        Vector3 scale = transform.localScale;
        scale.x = facingLeft ? -Mathf.Abs(scale.x) : Mathf.Abs(scale.x);
        transform.localScale = scale;
    }
}
