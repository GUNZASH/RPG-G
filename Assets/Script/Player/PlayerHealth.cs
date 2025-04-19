using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    public int potionCount = 0;
    public float maxHealth = 100f;
    public float health = 100f;

    public Image healthBar;

    private Animator anim;

    private bool isDead = false;

    void Start()
    {
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        UpdateHealthUI(); // ✅ เรียกทุกเฟรม
    }

    public void TakeDamage(float damage)
    {
        if (isDead) return; // ถ้าตายแล้วไม่รับดาเมจซ้ำ

        health -= damage;
        if (health < 0) health = 0;

        Debug.Log("Player HP: " + health);

        if (health <= 0)
        {
            Die();
        }
    }

    public void Heal(float amount)
    {
        health += amount;
        if (health > maxHealth)
        {
            health = maxHealth;
        }

        Debug.Log("Player Healed. Current HP: " + health);
    }

    void UpdateHealthUI()
    {
        if (healthBar != null)
        {
            healthBar.fillAmount = health / maxHealth;
        }
    }

    void Die()
    {
        if (isDead) return; // ป้องกันเรียกซ้ำ
        isDead = true;

        Debug.Log("You Die!");
        anim.SetTrigger("Die");
        StartCoroutine(EndGameAfterDeath());
    }

    System.Collections.IEnumerator EndGameAfterDeath()
    {
        yield return new WaitForSeconds(2f);
        //Time.timeScale = 0;
        DiePanelController.Instance.ShowDiePanel();
    }
}
