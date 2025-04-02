using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public int potionCount = 0;
    public float maxHealth = 100f;
    public float health = 100f;
    private Animator anim;

    void Start()
    {
        anim = GetComponent<Animator>();
    }

    public void TakeDamage(float damage)
    {
        health -= damage;
        Debug.Log("Player HP: " + health);

        if (health <= 0)
        {
            Die();
        }
    }

    public void Heal(float amount)
    {
        health += amount;
        if (health > maxHealth) // ห้ามให้เกิน maxHealth
        {
            health = maxHealth;
        }
        Debug.Log("Player Healed. Current HP: " + health);
    }

    void Die()
    {
        Debug.Log("You Die!");
        anim.SetTrigger("Die");
        StartCoroutine(EndGameAfterDeath());
    }

    IEnumerator EndGameAfterDeath()
    {
        yield return new WaitForSeconds(2f); // รออนิเมชั่นตายจบก่อนปิดเกม
        Time.timeScale = 0;
    }
}