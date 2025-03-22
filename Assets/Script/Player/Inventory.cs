using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
{
    public GameObject inventoryPanel;
    public TextMeshProUGUI statText;

    public Transform itemGrid; // Parent ของช่องเก็บของ (20 ช่อง)
    public GameObject inventorySlotPrefab; // Prefab ช่องเก็บของ

    public Image weaponSlot;
    public Image helmetSlot;
    public Image armorSlot;

    // เพิ่มปุ่มสำหรับอัปสเตตัส
    public Button atkButton;
    public Button vitButton;
    public Button agiButton;

    private bool isOpen = false;
    private LevelUp playerStats;

    void Start()
    {
        playerStats = GameObject.FindGameObjectWithTag("Player").GetComponent<LevelUp>();
        inventoryPanel.SetActive(false);
        GenerateInventorySlots();
        UpdateStatsUI();

        // เชื่อมโยงฟังก์ชันที่จะแก้ไขค่าของสเตตัส
        atkButton.onClick.AddListener(UpgradeATK);
        vitButton.onClick.AddListener(UpgradeVIT);
        agiButton.onClick.AddListener(UpgradeAGI);

        // ซ่อนปุ่มเริ่มต้น
        HideStatUpgradeButtons();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            isOpen = !isOpen;
            inventoryPanel.SetActive(isOpen);
            UpdateStatsUI();
        }

        // แสดงปุ่มสำหรับอัปสเตตัสหากมีคะแนนอัปเกรด
        if (playerStats.upgradePoints > 0)
        {
            ShowStatUpgradeButtons();
        }
        else
        {
            HideStatUpgradeButtons();
        }
    }

    void GenerateInventorySlots()
    {
        for (int i = 0; i < 20; i++)
        {
            Instantiate(inventorySlotPrefab, itemGrid);
        }
    }

    public void UpdateStatsUI()
    {
        statText.text = $"<b>ATK:</b> {playerStats.ATK} <color=#00FF00>(+{playerStats.bonusATK})</color>\n" +
                        $"<b>VIT:</b> {playerStats.VIT} <color=#00FF00>(+{playerStats.bonusVIT})</color>\n" +
                        $"<b>AGI:</b> {playerStats.AGI} <color=#00FF00>(+{playerStats.bonusAGI})</color>";
    }

    // ฟังก์ชันเพิ่มสเตตัสเมื่อกดปุ่ม
    void UpgradeATK()
    {
        if (playerStats.upgradePoints > 0)
        {
            playerStats.UpgradeStat("ATK");  // เปลี่ยนจาก UpgradeATK เป็น UpgradeStat
            UpdateStatsUI();
        }
    }

    void UpgradeVIT()
    {
        if (playerStats.upgradePoints > 0)
        {
            playerStats.UpgradeStat("VIT");  // เปลี่ยนจาก UpgradeVIT เป็น UpgradeStat
            UpdateStatsUI();
        }
    }

    void UpgradeAGI()
    {
        if (playerStats.upgradePoints > 0)
        {
            playerStats.UpgradeStat("AGI");  // เปลี่ยนจาก UpgradeAGI เป็น UpgradeStat
            UpdateStatsUI();
        }
    }

    // ฟังก์ชันแสดงปุ่มอัปสเตตัส
    void ShowStatUpgradeButtons()
    {
        atkButton.gameObject.SetActive(true);
        vitButton.gameObject.SetActive(true);
        agiButton.gameObject.SetActive(true);
    }

    // ฟังก์ชันซ่อนปุ่มอัปสเตตัส
    void HideStatUpgradeButtons()
    {
        atkButton.gameObject.SetActive(false);
        vitButton.gameObject.SetActive(false);
        agiButton.gameObject.SetActive(false);
    }
}