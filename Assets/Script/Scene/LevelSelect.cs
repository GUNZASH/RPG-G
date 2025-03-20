using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LevelSelect : MonoBehaviour
{
    [Header("ปุ่มเลือกด่าน")]
    public Button level1Button;
    public Button level2Button;
    public Button level3Button;

    [Header("เทส")]
    [SerializeField] private bool level1Complete;
    [SerializeField] private bool level2Complete;

    void Start()
    {
        level1Complete = PlayerPrefs.GetInt("Level1Complete", 0) == 1 || level1Complete;
        level2Complete = PlayerPrefs.GetInt("Level2Complete", 0) == 1 || level2Complete;

        UpdateLevelButtons();
    }

    void UpdateLevelButtons()
    {
        //ด่าน 1 กดได้เลย
        level1Button.interactable = true;

        //สีปุ่มที่ยังเล่นไม่ได้
        Color lockedColor = new Color(1f, 1f, 1f, 0.5f); //เปลี่ยนได้
        //สีปุ่มปกติ
        Color unlockedColor = Color.white;

        //ด่าน 2 จะเล่นได้ถ้าผ่านด่าน 1
        if (level1Complete)
        {
            level2Button.interactable = true;
            level2Button.GetComponent<Image>().color = unlockedColor; //สีปกติ
        }
        else
        {
            level2Button.interactable = false;
            level2Button.GetComponent<Image>().color = lockedColor; //สีทึบ
        }

        //ด่าน 3 จะเล่นได้ถ้าผ่านด่าน 1 2
        if (level1Complete && level2Complete)
        {
            level3Button.interactable = true;
            level3Button.GetComponent<Image>().color = unlockedColor; //สีปกติ
        }
        else
        {
            level3Button.interactable = false;
            level3Button.GetComponent<Image>().color = lockedColor; //สีทึบ
        }
    }

    public void LoadLevel(int levelNumber)
    {
        if (levelNumber == 1) SceneManager.LoadScene("Stage1");
        if (levelNumber == 2) SceneManager.LoadScene("Stage2");
        if (levelNumber == 3) SceneManager.LoadScene("Stage3");
    }

    //ใช้สำหรับบันทึกตอนผ่านด่าน
    public void MarkLevelComplete(int levelNumber)
    {
        if (levelNumber == 1)
        {
            level1Complete = true;
            PlayerPrefs.SetInt("Level1Complete", 1);
        }
        else if (levelNumber == 2)
        {
            level2Complete = true;
            PlayerPrefs.SetInt("Level2Complete", 1);
        }

        PlayerPrefs.Save();
        UpdateLevelButtons();
    }
}