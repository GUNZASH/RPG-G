using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TeleportPoint : MonoBehaviour
{
    public string sceneName; // กำหนดชื่อ Scene ที่ต้องการวาร์ปไป
    private bool canTeleport = false; // ตรวจสอบว่าผู้เล่นอยู่ในจุดเทเลพอร์ตหรือไม่

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
            SceneManager.LoadScene(sceneName); // โหลด Scene ใหม่
        }
    }
}