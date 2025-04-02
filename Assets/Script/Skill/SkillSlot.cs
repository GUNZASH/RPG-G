using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems; // ใช้สำหรับตรวจจับเมาส์

public class SkillSlot : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public string skillName;
    public string skillDescription;
    public int skillLevel = 0;
    public int maxLevel = 5;
    public SkillSlot prerequisiteSkill; // สกิลที่ต้องอัปก่อน
    public Image skillIcon;
    public Button upgradeButton;
    public Button equipButton;
    public GameObject skillDetailPanel;
    public TMP_Text skillDetailText;
    private GameObject equippedSkillIcon;

    private void Start()
    {
        if (upgradeButton != null)
            upgradeButton.onClick.AddListener(() => SkillSystem.Instance.UpgradeSkill(this));

        if (equipButton != null)
            equipButton.onClick.AddListener(() => SkillSystem.Instance.EquipSkill(this));

        if (skillDetailPanel != null)
            skillDetailPanel.SetActive(false);

        UpdateUI();
    }

    public void Upgrade()
    {
        if (CanUpgrade())
        {
            skillLevel++;
            UnlockSkill();
            UpdateUI();
            Debug.Log($"[SkillSlot] {skillName} อัปสกิลสำเร็จ! Level: {skillLevel}");
        }
        else
        {
            Debug.LogWarning($"[SkillSlot] {skillName} ไม่สามารถอัปสกิลได้! Level: {skillLevel}");
        }
    }

    public bool CanUpgrade()
    {
        bool hasSkillPoints = SkillSystem.Instance.skillPoints > 0;
        bool meetsPrerequisite = (prerequisiteSkill == null || prerequisiteSkill.skillLevel > 0);
        bool isUnderMaxLevel = skillLevel < maxLevel;

        bool canUpgrade = hasSkillPoints && meetsPrerequisite && isUnderMaxLevel;
        Debug.Log($"[SkillSlot] CanUpgrade {skillName}: {canUpgrade} (SkillPoints: {SkillSystem.Instance.skillPoints})");

        return canUpgrade;
    }

    public void Equip()
    {
        equipButton.interactable = false; // ปิดปุ่ม
    }

    public void Unequip()
    {
        equipButton.interactable = true; // เปิดปุ่ม
        if (equippedSkillIcon != null) Destroy(equippedSkillIcon);
    }

    public void CreateSkillIcon(Transform parent)
    {
        GameObject skillIconPrefab = new GameObject(skillName);
        skillIconPrefab.transform.SetParent(parent);
        equippedSkillIcon = skillIconPrefab;
    }

    public void LockSkill()
    {
        skillIcon.color = new Color(1, 1, 1, 0.3f);
        upgradeButton.interactable = false;
    }

    public void UnlockSkill()
    {
        skillIcon.color = new Color(1, 1, 1, 1);
        upgradeButton.interactable = true;
    }

    private void UpdateUI()
    {
        if (skillDetailText != null)
            skillDetailText.text = $"{skillName} (Lv {skillLevel}/{maxLevel})\n{skillDescription}";

        if (equipButton != null)
            equipButton.interactable = skillLevel > 0;

        if (skillLevel > 0)
        {
            UnlockSkill();
        }
    }

    // เมื่อชี้เมาส์ที่ไอคอนสกิล
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (skillDetailPanel != null)
        {
            skillDetailPanel.SetActive(true);
        }
    }

    // เมื่อเอาเมาส์ออกจากไอคอนสกิล
    public void OnPointerExit(PointerEventData eventData)
    {
        if (skillDetailPanel != null)
        {
            skillDetailPanel.SetActive(false);
        }
    }
}
