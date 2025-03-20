using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [Header("EnemySetting")]
    public float moveSpeed = 3f;
    public float attackRange = 2f;
    public float attackDamage = 10f;
    public float health = 100f;
    public float attackCooldown = 1f;

    private bool isAttacking = false;
    private Transform player;
    private float nextAttackTime = 0f;

    public GameObject dropItemPrefab; //ไอเท็มที่ดร็อป เผื่อจะทำ
    public Transform dropPoint; //จุดที่ไอเท็มจะตก เผื่อจะทำ

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform; //หาผู้เล่น
    }

    void Update()
    {
        float distanceToPlayer = Vector2.Distance(transform.position, player.position);

        if (distanceToPlayer <= attackRange && Time.time >= nextAttackTime)
        {
            AttackPlayer();
        }
        else if (distanceToPlayer <= attackRange)
        {
            MoveTowardsPlayer();
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

    public void DropItem() //เผื่อทำเรื่องดร็อป
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
        Destroy(gameObject);
        DropItem(); //เผื่อจะทำดร็อปไอเท็ม
    }

    void Flip(bool facingRight)
    {
        Vector3 scale = transform.localScale;
        scale.x = facingRight ? Mathf.Abs(scale.x) : -Mathf.Abs(scale.x);
        transform.localScale = scale;
    }
}