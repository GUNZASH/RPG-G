using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class NPCInteract : MonoBehaviour
{
    public GameObject dialoguePanel; // UI Panel สำหรับแสดงข้อความ
    public TextMeshProUGUI dialogueText; // ข้อความของ NPC
    [TextArea(3, 5)]
    public string npcDialogue = "สวัสดี! มีอะไรให้ช่วยไหม?"; // ข้อความที่ NPC จะพูด

    private bool isPlayerInRange = false;

    private void Start()
    {
        if (dialoguePanel != null)
        {
            dialoguePanel.SetActive(false); // ซ่อน UI Panel ตอนเริ่มเกม
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player")) // ตรวจสอบว่า Player เข้าใกล้ NPC หรือไม่
        {
            isPlayerInRange = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player")) // ตรวจสอบว่า Player ออกจากพื้นที่
        {
            isPlayerInRange = false;
            if (dialoguePanel != null)
            {
                dialoguePanel.SetActive(false); // ปิด UI Panel
            }
        }
    }

    private void Update()
    {
        if (isPlayerInRange && Input.GetKeyDown(KeyCode.W)) // ถ้าผู้เล่นอยู่ในระยะและกด W
        {
            ShowDialogue();
        }
    }

    private void ShowDialogue()
    {
        if (dialoguePanel != null && dialogueText != null)
        {
            dialoguePanel.SetActive(true); // แสดง UI Panel
            dialogueText.text = npcDialogue; // อัปเดตข้อความ NPC
        }
    }
}