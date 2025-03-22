using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class InventorySlot : MonoBehaviour
{
    public Image icon;
    private Item storedItem;

    public void AddItem(Item newItem)
    {
        storedItem = newItem;
        icon.sprite = newItem.icon;
        icon.enabled = true;
    }

    public void ClearSlot()
    {
        storedItem = null;
        icon.sprite = null;
        icon.enabled = false;
    }

    public void OnSlotClicked()
    {
        if (storedItem != null)
        {
            Debug.Log("Clicked on: " + storedItem.itemName);
        }
    }
}