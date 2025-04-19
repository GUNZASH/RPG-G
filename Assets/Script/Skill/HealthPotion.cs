using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HealthPotion : MonoBehaviour
{
    public float healAmount = 50f; // ปริมาณการฮีล
    public float cooldownTime = 60f; // คูลดาวน์ 60 วิ

    public Button potionButton; // ปุ่มกดยา
    public TMP_Text potionText; // ข้อความแสดงจำนวนยา

    private PlayerHealth playerHealth;
    private bool isCooldown = false;

    private void Start()
    {
        playerHealth = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerHealth>();
        UpdatePotionUI();
        potionButton.onClick.AddListener(UsePotion);
    }

    void UsePotion()
    {
        if (playerHealth.potionCount > 0 && !isCooldown)
        {
            playerHealth.Heal(healAmount); // ใช้ Heal() โดยตรง
            playerHealth.potionCount--;
            UpdatePotionUI();
            StartCoroutine(CooldownCoroutine());
        }
    }

    void UpdatePotionUI()
    {
        if (potionText != null)
        {
            potionText.text = playerHealth.potionCount.ToString(); // ใช้ค่าจาก PlayerHealth
        }
    }

    IEnumerator CooldownCoroutine()
    {
        isCooldown = true;
        potionButton.interactable = false; // ปิดปุ่มระหว่างคูลดาวน์
        yield return new WaitForSeconds(cooldownTime);
        isCooldown = false;
        potionButton.interactable = true; // เปิดปุ่มให้กดได้อีกครั้ง
    }

    private void Update()
    {
        UpdatePotionUI(); // อัปเดต UI ตลอดเวลา
    }
}