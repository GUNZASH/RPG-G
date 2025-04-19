using System.Collections;
using UnityEngine;
using TMPro; // ใช้สำหรับข้อความ Loading
using UnityEngine.UI; // ใช้สำหรับ Panel

public class TeleportPoint : MonoBehaviour
{
    public Transform teleportDestination; // จุดที่ต้องการวาร์ปไป
    public GameObject loadingPanel; // Panel สีดำ
    public TextMeshProUGUI loadingText; // ข้อความ Loading . . .

    public Image[] imagesToHide;

    private bool canTeleport = false; // ตรวจสอบว่าผู้เล่นอยู่ในจุดเทเลพอร์ตหรือไม่
    private GameObject player; // อ้างอิงถึงตัวผู้เล่น

    private void Start()
    {
        loadingPanel.SetActive(false); // ปิด Panel ตั้งแต่เริ่มเกม
        player = GameObject.FindGameObjectWithTag("Player"); // หา Player ในฉาก
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player")) // ตรวจสอบว่า Player เข้าเขตเทเลพอร์ต
        {
            canTeleport = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player")) // ตรวจสอบว่า Player ออกจากเขตเทเลพอร์ต
        {
            canTeleport = false;
        }
    }

    private void Update()
    {
        if (canTeleport && Input.GetKeyDown(KeyCode.W)) // ถ้าอยู่ในจุดเทเลพอร์ตและกด W
        {
            StartCoroutine(TeleportSequence()); // เริ่มกระบวนการวาร์ป
        }
    }

    private IEnumerator TeleportSequence()
    {
        loadingPanel.SetActive(true); // เปิดจอดำ
        loadingText.text = "Loading . . ."; // แสดงข้อความ Loading

        foreach (Image img in imagesToHide)
        {
            img.gameObject.SetActive(false);
        }

        float waitTime = Random.Range(2f, 4f); // สุ่มรอ 2-4 วินาที
        yield return new WaitForSeconds(1f); // แสดงจอดำก่อนย้ายตำแหน่ง
        player.transform.position = teleportDestination.position; // ย้ายผู้เล่นไปตำแหน่งใหม่
        yield return new WaitForSeconds(waitTime - 1f); // รอเวลาที่เหลือ

        loadingPanel.SetActive(false); //
                                       // 
        foreach (Image img in imagesToHide)
        {
            img.gameObject.SetActive(true);
        }
    }
}
