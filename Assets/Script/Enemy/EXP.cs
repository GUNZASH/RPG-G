using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EXP : MonoBehaviour
{
    public int expReward = 50; // EXP ที่ให้เมื่อศัตรูตาย

    void OnDestroy()
    {
        LevelUp playerLevel = GameObject.FindGameObjectWithTag("Player").GetComponent<LevelUp>();
        if (playerLevel != null)
        {
            playerLevel.GainEXP(expReward);
        }
    }
}