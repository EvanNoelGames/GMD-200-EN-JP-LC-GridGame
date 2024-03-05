using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(fileName = "Item", menuName = "ScriptableObjects/Item", order = 1)]
public class ItemData : ScriptableObject
{
    public string itemName;
    public enum ItemType
    {
        weapon,
        armor,
        health,
        special
    }

    public enum Rarity
    {
        common,
        uncommon,
        rare
    }

    public ItemType itemType;
    public Rarity rarity;
    public int damage;
    public int defence;
    public int healing;

    public Sprite sprite;
}
