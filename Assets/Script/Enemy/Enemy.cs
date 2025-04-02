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
    public float patrolDistance = 3f; // ระยะทางที่เดินไปมาก่อนเปลี่ยนทิศ
    private Vector2 startPos;
    private int patrolDirection = 1; // 1 = เดินไปขวา, -1 = เดินไปซ้าย

    void Start()
    {
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

        if (health <= 0)
        {
            Die();
        }
    }

    void MoveTowardsPlayer()
    {
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
            nextAttackTime = Time.time + attackCooldown;

            player.GetComponent<PlayerHealth>().TakeDamage(attackDamage);

            Invoke("ResetAttack", 0.5f);
        }
    }

    void ResetAttack()
    {
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
        QuestTracker.Instance.EnemyKilled(gameObject.tag); // แจ้งให้ QuestTracker รู้ว่าศัตรูตายแล้ว
        Destroy(gameObject);
        DropItem(); // เผื่อจะทำดร็อปไอเท็ม
    }

    void Flip(bool facingRight)
    {
        Vector3 scale = transform.localScale;
        scale.x = facingRight ? Mathf.Abs(scale.x) : -Mathf.Abs(scale.x);
        transform.localScale = scale;
    }
}
