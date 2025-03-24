using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class QuestTracker : MonoBehaviour
{
    public static QuestTracker Instance;

    public GameObject questTrackerPanel; // UI Panel ติดตามเควส
    public TMP_Text questListText; // แสดงเควสที่กำลังทำอยู่

    private Dictionary<string, int> questProgress = new Dictionary<string, int>(); // นับศัตรูที่ถูกฆ่า
    private Dictionary<string, QuestData> activeQuests = new Dictionary<string, QuestData>(); // เก็บข้อมูลเควสที่กำลังทำ

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    private void Start()
    {
        questTrackerPanel.SetActive(false);
    }

    public void StartQuest(QuestData quest)
    {
        if (!activeQuests.ContainsKey(quest.requiredEnemyTag))
        {
            activeQuests.Add(quest.requiredEnemyTag, quest);
            questProgress[quest.requiredEnemyTag] = 0;

            questTrackerPanel.SetActive(true);
            UpdateQuestList();
        }
    }

    public void EnemyKilled(string enemyTag)
    {
        if (activeQuests.ContainsKey(enemyTag))
        {
            questProgress[enemyTag]++;

            if (questProgress[enemyTag] >= activeQuests[enemyTag].requiredAmount)
            {
                CompleteQuest(enemyTag);
            }

            UpdateQuestList();
        }
    }

    private void CompleteQuest(string enemyTag)
    {
        QuestData quest = activeQuests[enemyTag];

        // แจก EXP และ Gold
        LevelUp.Instance.GainEXP(quest.rewardEXP);
        LevelUp.Instance.GainGold(quest.rewardGold);

        Debug.Log($"เควส {quest.questName} สำเร็จ! ได้รับ {quest.rewardEXP} EXP และ {quest.rewardGold} Gold");

        activeQuests.Remove(enemyTag);
        questProgress.Remove(enemyTag);

        if (activeQuests.Count == 0) questTrackerPanel.SetActive(false);
    }

    private void UpdateQuestList()
    {
        questListText.text = "";
        foreach (var quest in activeQuests)
        {
            string enemyTag = quest.Key;
            string questName = quest.Value.questName;
            int progress = questProgress[enemyTag];
            int target = quest.Value.requiredAmount;

            questListText.text += $"{questName}: {progress}/{target}\n";
        }
    }
}