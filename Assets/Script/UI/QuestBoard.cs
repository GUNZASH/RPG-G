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

    private void ShowQuestDetail(int index)
    {
        if (index >= 0 && index < questDataList.Count)
        {
            selectedQuestIndex = index;
            QuestData quest = questDataList[index];

            questNameText.text = quest.questName;
            questDescriptionText.text = quest.description;
            questDetailImage.sprite = quest.questImage;

            questDetailPanel.SetActive(true);
        }
    }

    private void AcceptQuest()
    {
        if (selectedQuestIndex != -1)
        {
            questButtons[selectedQuestIndex].gameObject.SetActive(false); // ซ่อนเควสที่เลือก
            questDetailPanel.SetActive(false); // ปิดหน้ารายละเอียด
        }
    }

    private void CloseQuestDetail()
    {
        questDetailPanel.SetActive(false);
    }
}
