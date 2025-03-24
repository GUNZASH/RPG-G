using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LevelUp : MonoBehaviour
{
    public static LevelUp Instance; // เพิ่ม Instance

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

    public int gold = 0; // เพิ่มตัวแปรทอง

    public int rankEXP = 0; // เพิ่มตัวแปร Rank EXP
    public int rankLevel = 1; // เพิ่มตัวแปร Rank Level
    public int rankEXPToNext = 500; // จำนวน Rank EXP ที่ต้องใช้ในการเลื่อนระดับ Rank

    public TMP_Text rankText; // TMP_Text ที่ใช้แสดง Rank

    private PlayerController playerController;
    private PlayerHealth playerHealth;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    void Start()
    {
        // เชื่อมโยง PlayerController และ PlayerHealth
        playerController = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        playerHealth = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerHealth>();
    }

    void Update()
    {
        // อัปเดตข้อความ Rank
        if (rankText != null)
        {
            string rankName = GetRankName(); // รับชื่อแรงค์
            rankText.text = "Rank: " + rankName + " (" + rankEXP + "/" + rankEXPToNext.ToString() + ")";
        }
    }

    public void GainEXP(int amount)
    {
        exp += amount;
        CheckLevelUp();
        GainRankEXP(amount); // เรียกใช้ฟังก์ชันเพิ่ม Rank EXP
    }

    public void GainGold(int amount)
    {
        gold += amount; // เพิ่มทอง
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

    public void GainRankEXP(int amount)
    {
        rankEXP += amount;
        while (rankEXP >= rankEXPToNext)
        {
            rankEXP -= rankEXPToNext;
            rankLevel++;
            rankEXPToNext = Mathf.RoundToInt(rankEXPToNext * 1.5f); // เพิ่ม Rank EXP ที่ต้องใช้เมื่อเลื่อน rank
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
    public int GetGold() => gold; // ฟังก์ชันเรียกทอง

    // ฟังก์ชันที่ใช้แปลง Rank Level เป็นชื่อแรงค์
    string GetRankName()
    {
        if (rankLevel >= 1 && rankLevel <= 4) return "Bronze";
        if (rankLevel >= 5 && rankLevel <= 9) return "Silver";
        if (rankLevel >= 10 && rankLevel <= 14) return "Gold";
        if (rankLevel >= 15 && rankLevel <= 19) return "Platinum";
        if (rankLevel >= 20) return "Diamond";
        return "Bronze"; // ถ้าไม่ตรงกับเงื่อนไขใดๆ ให้แสดง Bronze
    }
}