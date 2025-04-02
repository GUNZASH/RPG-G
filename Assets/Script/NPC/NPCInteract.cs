using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class NPCInteract : MonoBehaviour
{
    public GameObject dialoguePanel; // UI Panel สำหรับแสดงข้อความ
    public TextMeshProUGUI dialogueText; // ข้อความของ NPC
    [TextArea(3, 5)]
    public string npcDialogue = "สวัสดี! มีอะไรให้ช่วยไหม?"; // ข้อความที่ NPC จะพูด

    private bool isPlayerInRange = false;

    public GameObject buyPanel; // Panel สำหรับปุ่มซื้อ
    public Button buy1PotionButton;
    public Button buy3PotionButton;
    public Button closeButton;
    public TextMeshProUGUI buy1PotionText;
    public TextMeshProUGUI buy3PotionText;

    private PlayerHealth playerHealth; // อ้างอิงไปที่ PlayerHealth

    private void Start()
    {
        if (dialoguePanel != null) dialoguePanel.SetActive(false);
        if (buyPanel != null) buyPanel.SetActive(false);
        
        playerHealth = FindObjectOfType<PlayerHealth>(); // หา PlayerHealth ในฉาก
        
        if (buy1PotionButton != null) buy1PotionButton.onClick.AddListener(() => BuyPotion(1, 500));
        if (buy3PotionButton != null) buy3PotionButton.onClick.AddListener(() => BuyPotion(3, 1400));
        if (closeButton != null) closeButton.onClick.AddListener(CloseDialogue);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player")) isPlayerInRange = true;
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInRange = false;
            CloseDialogue();
        }
    }

    private void Update()
    {
        if (isPlayerInRange && Input.GetKeyDown(KeyCode.W))
        {
            ShowDialogue();
        }

        UpdateButtonState();
    }

    private void ShowDialogue()
    {
        if (dialoguePanel != null && dialogueText != null)
        {
            dialoguePanel.SetActive(true);
            buyPanel.SetActive(true);
            dialogueText.text = npcDialogue;
            
            buy1PotionText.text = "ซื้อ Potion 1 ขวด (500 Gold)";
            buy3PotionText.text = "ซื้อ Potion 3 ขวด (1400 Gold)";
            
            UpdateButtonState();
        }
    }

    private void CloseDialogue()
    {
        if (dialoguePanel != null) dialoguePanel.SetActive(false);
        if (buyPanel != null) buyPanel.SetActive(false);
    }

    private void BuyPotion(int amount, int cost)
    {
        if (LevelUp.Instance.gold >= cost && playerHealth != null)
        {
            LevelUp.Instance.gold -= cost;
            playerHealth.potionCount += amount; // เพิ่มจำนวนยาใน PlayerHealth
            Debug.Log("ซื้อ Potion " + amount + " ขวดแล้ว! เหลือทอง: " + LevelUp.Instance.gold);
        }
        else
        {
            Debug.Log("เงินไม่พอ!");
        }
    }

    private void UpdateButtonState()
    {
        if (buy1PotionButton != null) buy1PotionButton.interactable = (LevelUp.Instance.gold >= 500);
        if (buy3PotionButton != null) buy3PotionButton.interactable = (LevelUp.Instance.gold >= 1400);
    }
}
