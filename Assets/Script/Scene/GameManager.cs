using UnityEngine;

public class GameManager : MonoBehaviour
{
    void Start()
    {
        // โหลดข้อมูลเมื่อ Scene ใหม่ (เช่น GamePlay) เริ่มต้น
        SaveManager.Instance.LoadGame();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Debug.Log("ESC Pressed"); // เพิ่ม Debug เพื่อตรวจสอบว่าปุ่มถูกกด
            SaveManager.Instance.SaveGame();

#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
        }
    }

}
