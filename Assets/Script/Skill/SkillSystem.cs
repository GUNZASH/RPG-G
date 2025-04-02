using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SkillSystem : MonoBehaviour
{
    public static SkillSystem Instance; // Singleton

    [Header("Skill UI")]
    public GameObject skillPanel; // Panel หลัก
    public List<SkillSlot> skillSlots; // รายการสกิล
    public Transform equippedSkillBar; // ที่อยู่ของสกิลที่ติดตั้ง (UI ด้านล่าง)

    [Header("Player Skill Data")]
    public int skillPoints = 0; // แต้มสกิลที่มี
    private List<SkillSlot> equippedSkills = new List<SkillSlot>(); // สกิลที่ติดตั้ง

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    private void Start()
    {
        // เช็คทุกสกิลให้เป็นสีทึบ (ยังไม่อัป)
        foreach (var slot in skillSlots)
        {
            slot.LockSkill();
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.K))
        {
            ToggleSkillPanel();
        }
    }

    public void ToggleSkillPanel()
    {
        skillPanel.SetActive(!skillPanel.activeSelf);
    }

    public void GainSkillPoint(int amount)
    {
        skillPoints += amount;
    }

    public void UpgradeSkill(SkillSlot skill)
    {
        if (skillPoints > 0 && skill.CanUpgrade())
        {
            skillPoints--; // ใช้ Skill Point
            skill.Upgrade();
            Debug.Log($"[SkillSystem] อัปเกรดสกิล {skill.skillName} สำเร็จ! เหลือ Skill Points: {skillPoints}");
        }
        else
        {
            Debug.LogWarning($"[SkillSystem] อัปเกรดสกิล {skill.skillName} ไม่ได้! Skill Points: {skillPoints}");
        }
    }

    public void EquipSkill(SkillSlot skill)
    {
        if (equippedSkills.Count < 5 && !equippedSkills.Contains(skill))
        {
            equippedSkills.Add(skill);
            skill.Equip();
            Debug.Log($"[SkillSystem] ใส่สกิล {skill.skillName}");
        }
    }

    public void UnequipSkill(SkillSlot skill)
    {
        if (equippedSkills.Contains(skill))
        {
            equippedSkills.Remove(skill);
            skill.Unequip();
            Debug.Log($"[SkillSystem] ถอดสกิล {skill.skillName}");
        }
    }
}