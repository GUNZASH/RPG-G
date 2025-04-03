using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

[System.Serializable]
public class QuestData
{
    public string questName; // ชื่อเควส
    public string description; // รายละเอียดเควส
    public Sprite questImage; // ภาพรายละเอียดของเควส
    public string requiredEnemyTag; // ศัตรูที่ต้องฆ่า (กำหนดใน Inspector)
    public int requiredAmount; // จำนวนที่ต้องฆ่า
    public int rewardEXP; // ค่าประสบการณ์ที่ได้รับ
    public int rewardGold; // ทองที่ได้รับ
    public int rankRequirement; // เพิ่มตัวแปร Rank Requirement (เงื่อนไขระดับแรงค์ในการรับเควส)
}

public class QuestBoard : MonoBehaviour
{
    public GameObject questPanel; // UI Panel หลักของกระดานเควส
    public List<Button> questButtons; // ปุ่มเควส (กระดาษที่สามารถคลิกได้)
    public List<QuestData> questDataList; // รายละเอียดของแต่ละเควส

    public GameObject questDetailPanel; // UI Panel รายละเอียดเควส
    public TMP_Text questNameText; // ชื่อเควส (ใช้ TMPro)
    public TMP_Text questDescriptionText; // คำอธิบายเควส (ใช้ TMPro)
    public Image questDetailImage; // ภาพเควสในหน้ารายละเอียด

    public Button acceptButton; // ปุ่มยอมรับเควส
    public Button cancelButton; // ปุ่มยกเลิก

    private bool isPlayerInRange = false;
    private int selectedQuestIndex = -1; // เก็บค่า index ของเควสที่ถูกเลือก

    public QuestTracker questTracker; // เชื่อมกับระบบติดตามเควส
    public LevelUp levelUp; // เชื่อมกับระบบ Rank ที่มีอยู่แล้ว

    private void Start()
    {
        questPanel.SetActive(false);
        questDetailPanel.SetActive(false);

        for (int i = 0; i < questButtons.Count; i++)
        {
            int index = i; // ป้องกันปัญหา Lambda Expression
            questButtons[i].onClick.AddListener(() => ShowQuestDetail(index));
        }

        acceptButton.onClick.AddListener(AcceptQuest);
        cancelButton.onClick.AddListener(CloseQuestDetail);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInRange = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInRange = false;
            questPanel.SetActive(false);
            questDetailPanel.SetActive(false);
        }
    }

    private void Update()
    {
        if (isPlayerInRange && Input.GetKeyDown(KeyCode.W))
        {
            questPanel.SetActive(true);
        }
    }

    // เพิ่มฟังก์ชันที่ใช้แปลง rankRequirement เป็นชื่อ Rank
    public string GetQuestRankName(int rankRequirement)
    {
        if (rankRequirement >= 1 && rankRequirement <= 4) return "Bronze";
        if (rankRequirement >= 5 && rankRequirement <= 9) return "Silver";
        if (rankRequirement >= 10 && rankRequirement <= 14) return "Gold";
        if (rankRequirement >= 15 && rankRequirement <= 19) return "Platinum";
        if (rankRequirement >= 20) return "Diamond";
        return "Bronze"; // ถ้าไม่ตรงกับเงื่อนไขใดๆ ให้แสดง Bronze
    }

    private void ShowQuestDetail(int index)
    {
        if (index >= 0 && index < questDataList.Count)
        {
            selectedQuestIndex = index;
            QuestData quest = questDataList[index];

            questNameText.text = quest.questName;
            questDescriptionText.text = quest.description;
            questDetailImage.sprite = quest.questImage;

            // แสดง Rank ของเควสจาก rankRequirement
            string questRank = GetQuestRankName(quest.rankRequirement); // ใช้ rankRequirement ของเควส
            questDescriptionText.text += "\nRequired Rank: " + questRank;

            // ตรวจสอบว่า Rank ของผู้เล่นถึงระดับที่ต้องการสำหรับเควสนี้หรือไม่
            if (levelUp.rankLevel >= quest.rankRequirement)
            {
                acceptButton.interactable = true; // เปิดปุ่มรับเควส
            }
            else
            {
                acceptButton.interactable = false; // ปิดปุ่มรับเควส
                questDescriptionText.text += "\nYour rank is too low to accept this quest!";
            }

            questDetailPanel.SetActive(true);
        }
    }

    private void AcceptQuest()
    {
        if (selectedQuestIndex != -1)
        {
            QuestData selectedQuest = questDataList[selectedQuestIndex];
            QuestTracker.Instance.StartQuest(selectedQuest);

            questButtons[selectedQuestIndex].gameObject.SetActive(false);
            questDetailPanel.SetActive(false);
        }
    }

    private void CloseQuestDetail()
    {
        questDetailPanel.SetActive(false);
    }
}
