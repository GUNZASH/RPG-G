using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EXP : MonoBehaviour
{
    public int expReward = 50; // EXP ที่ให้เมื่อศัตรูตาย

    void OnDestroy()
    {
        GameObject playerObject = GameObject.FindGameObjectWithTag("Player");

        if (playerObject != null)
        {
            LevelUp playerLevel = playerObject.GetComponent<LevelUp>();
            if (playerLevel != null)
            {
                playerLevel.GainEXP(expReward);
            }
            else
            {
                Debug.LogWarning("LevelUp component not found on Player!");
            }
        }
        else
        {
            Debug.LogWarning("Player not found with tag 'Player'!");
        }
    }
}