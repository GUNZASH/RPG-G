using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Item", menuName = "Inventory/Item")]
public class Item : ScriptableObject
{
    public string itemName;
    public Sprite icon;
    public ItemType itemType;
    public int atkBonus;
    public int vitBonus;
    public int agiBonus;
}

public enum ItemType { Weapon, Helmet, Armor, Consumable }