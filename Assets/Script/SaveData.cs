using System;
using System.Collections.Generic;

[Serializable]
public class SaveData
{
    // จาก LevelUp
    public int level;
    public int exp;
    public int expToNextLevel;
    public int upgradePoints;
    public int gold;
    public int rankEXP;
    public int rankLevel;
    public int rankEXPToNext;

    public int ATK, VIT, AGI;
    public int bonusATK, bonusVIT, bonusAGI;

    // จาก PlayerHealth
    public int potionCount;
    public float maxHealth;

    // จาก PlayerController
    public float moveSpeed;
    public float attackDamage;

    // Quest ที่รับอยู่
    public List<string> acceptedQuests = new List<string>();
    public List<int> questProgress = new List<int>(); // ถ้ามีการนับว่าฆ่าไปกี่ตัวแล้ว

    // ตำแหน่งผู้เล่น (Vector3 → float[3])
    public float[] playerPosition = new float[3];
}
