using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelUp : MonoBehaviour
{
    public int level = 1;
    public int exp = 0;
    public int expToNextLevel = 100;
    public int upgradePoints = 0;

    public int baseExp = 100; // EXP ที่ต้องใช้ในการอัปเลเวล
    public float expMultiplier = 1.2f; // คูณ EXP ที่ต้องใช้เพิ่มขึ้นทุกเลเวล

    public int ATK = 1;
    public int VIT = 1;
    public int AGI = 1;

    public int bonusATK = 0;
    public int bonusVIT = 0;
    public int bonusAGI = 0;

    private PlayerController playerController;
    private PlayerHealth playerHealth;

    void Start()
    {
        // เชื่อมโยง PlayerController และ PlayerHealth
        playerController = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        playerHealth = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerHealth>();
    }

    public void GainEXP(int amount)
    {
        exp += amount;
        CheckLevelUp();
    }

    void CheckLevelUp()
    {
        while (exp >= expToNextLevel)
        {
            exp -= expToNextLevel;
            level++;
            upgradePoints += 2;
            expToNextLevel = Mathf.RoundToInt(baseExp * Mathf.Pow(expMultiplier, level - 1));
        }
    }

    public void UpgradeStat(string statType)
    {
        if (upgradePoints > 0)
        {
            switch (statType)
            {
                case "ATK":
                    ATK++;
                    bonusATK++;
                    playerController.attackDamage += 1; // เพิ่ม attackDamage ใน PlayerController
                    break;
                case "VIT":
                    VIT++;
                    bonusVIT++;
                    playerHealth.health += 10; // เพิ่ม health ใน PlayerHealth
                    break;
                case "AGI":
                    AGI++;
                    bonusAGI++;
                    playerController.moveSpeed += 0.5f; // เพิ่ม moveSpeed ใน PlayerController
                    playerController.dashSpeed += 1f; // เพิ่ม dashSpeed ใน PlayerController
                    playerController.dashCooldown -= 0.2f; // ลด dashCooldown ใน PlayerController
                    break;
            }
            upgradePoints--;
        }
    }

    public int GetTotalATK() => ATK + bonusATK;
    public int GetTotalVIT() => VIT + bonusVIT;
    public int GetTotalAGI() => AGI + bonusAGI;
}