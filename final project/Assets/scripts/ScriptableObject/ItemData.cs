using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ItemType
{
    NONE,
    PotatoSeeds, Potato,
    RadishSeeds, Radish,
    TurnipSeeds, Turnip,
    SpinachSeeds, Spinach,
    Water
}

[CreateAssetMenu(fileName = "Item Data", menuName = "Item Data", order = 50)]
[System.Serializable]
public class ItemData : ScriptableObject
{
    public string itemName;
    public Sprite icon;
    public ItemType type;
    public int maxAllowed = 99;
}
