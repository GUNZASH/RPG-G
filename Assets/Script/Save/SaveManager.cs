using System.IO;
using UnityEngine;
using System.Collections.Generic;
using System;

public class SaveManager : MonoBehaviour
{
    public static SaveManager Instance;

    public LevelUp levelUp;
    public PlayerHealth playerHealth;
    public PlayerController playerController;
    public QuestBoard questBoard;
    public Transform playerTransform;

    private string saveFilePath;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // ป้องกันไม่ให้โดนลบตอนเปลี่ยนซีน
        }
        else
        {
            Destroy(gameObject);
        }

        // ใช้เส้นทางที่คงที่และเข้าถึงได้ง่าย
        saveFilePath = Application.persistentDataPath + "/savefile.json";
        Debug.Log("Save File Path: " + saveFilePath); // ตรวจสอบเส้นทางไฟล์
                                                      // ----------- Auto-Assign if null ------------
        if (levelUp == null)
        {
            levelUp = FindObjectOfType<LevelUp>();
            Debug.Log("levelUp found: " + (levelUp != null));
        }

        if (playerHealth == null)
        {
            playerHealth = FindObjectOfType<PlayerHealth>();
            Debug.Log("playerHealth found: " + (playerHealth != null));
        }

        if (playerController == null)
        {
            playerController = FindObjectOfType<PlayerController>();
            Debug.Log("playerController found: " + (playerController != null));
        }

        if (questBoard == null)
        {
            questBoard = FindObjectOfType<QuestBoard>();
            Debug.Log("questBoard found: " + (questBoard != null));
            if (questBoard != null && questBoard.questTracker != null)
            {
                Debug.Log("questTracker found: TRUE");
            }
            else
            {
                Debug.LogWarning("questTracker is NULL");
            }
        }

        if (playerTransform == null)
        {
            GameObject playerObj = GameObject.FindWithTag("Player");
            if (playerObj != null)
            {
                playerTransform = playerObj.transform;
                Debug.Log("playerTransform found via tag: " + playerTransform.name);
            }
            else
            {
                Debug.LogWarning("playerTransform not found via tag!");
            }
        }
    }

    public void SaveGame()
    {
        Debug.Log("========== Saving Game ==========");

        // เช็กว่า object ไหน null ก่อนเริ่ม save
        Debug.Log("levelUp: " + (levelUp != null ? "OK" : "NULL"));
        Debug.Log("playerHealth: " + (playerHealth != null ? "OK" : "NULL"));
        Debug.Log("playerController: " + (playerController != null ? "OK" : "NULL"));
        Debug.Log("questBoard: " + (questBoard != null ? "OK" : "NULL"));
        Debug.Log("questTracker: " + (questBoard != null && questBoard.questTracker != null ? "OK" : "NULL"));
        Debug.Log("playerTransform: " + (playerTransform != null ? "OK" : "NULL"));

        SaveData data = new SaveData();

        // ---------- LevelUp ----------
        if (levelUp != null)
        {
            data.level = levelUp.level;
            data.exp = levelUp.exp;
            data.expToNextLevel = levelUp.expToNextLevel;
            data.upgradePoints = levelUp.upgradePoints;
            data.gold = levelUp.gold;
            data.rankEXP = levelUp.rankEXP;
            data.rankLevel = levelUp.rankLevel;
            data.rankEXPToNext = levelUp.rankEXPToNext;

            data.ATK = levelUp.ATK;
            data.VIT = levelUp.VIT;
            data.AGI = levelUp.AGI;
            data.bonusATK = levelUp.bonusATK;
            data.bonusVIT = levelUp.bonusVIT;
            data.bonusAGI = levelUp.bonusAGI;
        }
        else
        {
            Debug.LogError("❌ levelUp is NULL while saving!");
        }

        // ---------- PlayerHealth ----------
        if (playerHealth != null)
        {
            data.potionCount = playerHealth.potionCount;
            data.maxHealth = playerHealth.maxHealth;
        }
        else
        {
            Debug.LogError("❌ playerHealth is NULL while saving!");
        }

        // ---------- PlayerController ----------
        if (playerController != null)
        {
            data.moveSpeed = playerController.moveSpeed;
            data.attackDamage = playerController.attackDamage;
        }
        else
        {
            Debug.LogError("❌ playerController is NULL while saving!");
        }

        // ---------- Quest ----------
        if (questBoard != null && questBoard.questTracker != null)
        {
            foreach (var quest in questBoard.questDataList)
            {
                if (questBoard.questTracker.HasQuest(quest.questName))
                {
                    data.acceptedQuests.Add(quest.questName);
                    data.questProgress.Add(questBoard.questTracker.GetProgress(quest.questName));
                }
            }
        }
        else
        {
            Debug.LogError("❌ questBoard or questTracker is NULL while saving!");
        }

        // ---------- Player Position ----------
        if (playerTransform != null)
        {
            Vector3 pos = playerTransform.position;
            data.playerPosition[0] = pos.x;
            data.playerPosition[1] = pos.y;
            data.playerPosition[2] = pos.z;
        }
        else
        {
            Debug.LogError("❌ playerTransform is NULL while saving!");
        }

        // ---------- Save to file ----------
        string json = JsonUtility.ToJson(data, true);
        try
        {
            File.WriteAllText(saveFilePath, json);
            Debug.Log("✅ Game Saved to: " + saveFilePath);
        }
        catch (Exception ex)
        {
            Debug.LogError("❌ Error while saving game: " + ex.Message);
        }

        Debug.Log("========== Save Complete ==========");
    }

    public void LoadGame()
    {
        if (!File.Exists(saveFilePath))
        {
            Debug.LogWarning("No save file found at: " + saveFilePath); // แจ้งเตือนเมื่อไม่พบไฟล์เซฟ
            return;
        }

        try
        {
            string json = File.ReadAllText(saveFilePath);
            SaveData data = JsonUtility.FromJson<SaveData>(json);

            if (data == null)
            {
                Debug.LogError("SaveData is null after deserialization.");
                return;
            }

            // ---------- LevelUp ----------
            if (levelUp != null)
            {
                levelUp.level = data.level;
                levelUp.exp = data.exp;
                levelUp.expToNextLevel = data.expToNextLevel;
                levelUp.upgradePoints = data.upgradePoints;
                levelUp.gold = data.gold;
                levelUp.rankEXP = data.rankEXP;
                levelUp.rankLevel = data.rankLevel;
                levelUp.rankEXPToNext = data.rankEXPToNext;

                levelUp.ATK = data.ATK;
                levelUp.VIT = data.VIT;
                levelUp.AGI = data.AGI;
                levelUp.bonusATK = data.bonusATK;
                levelUp.bonusVIT = data.bonusVIT;
                levelUp.bonusAGI = data.bonusAGI;
            }
            else Debug.LogError("levelUp is NULL during LoadGame!");

            // ---------- PlayerHealth ----------
            if (playerHealth != null)
            {
                playerHealth.potionCount = data.potionCount;
                playerHealth.maxHealth = data.maxHealth;
            }
            else Debug.LogError("playerHealth is NULL during LoadGame!");

            // ---------- PlayerController ----------
            if (playerController != null)
            {
                playerController.moveSpeed = data.moveSpeed;
                playerController.attackDamage = data.attackDamage;
            }
            else Debug.LogError("playerController is NULL during LoadGame!");

            // ---------- Quest ----------
            if (questBoard != null && questBoard.questTracker != null)
            {
                questBoard.questTracker.ClearAll();
                for (int i = 0; i < data.acceptedQuests.Count; i++)
                {
                    string questName = data.acceptedQuests[i];
                    int progress = data.questProgress[i];
                    QuestData questData = questBoard.questDataList.Find(q => q.questName == questName);
                    if (questData != null)
                    {
                        questBoard.questTracker.AcceptQuest(questData.requiredEnemyTag, questData, progress);
                    }
                }
            }
            else Debug.LogError("questBoard or questTracker is NULL during LoadGame!");

            // ---------- Player Position ----------
            if (playerTransform != null && data.playerPosition != null && data.playerPosition.Length == 3)
            {
                playerTransform.position = new Vector3(
                    data.playerPosition[0],
                    data.playerPosition[1],
                    data.playerPosition[2]
                );
            }
            else Debug.LogError("playerTransform or playerPosition is NULL or malformed during LoadGame!");

            Debug.Log("Game Loaded from: " + saveFilePath);
        }
        catch (Exception ex)
        {
            Debug.LogError("Error while loading game: " + ex.Message);
        }
    }
}
