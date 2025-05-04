using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ItemType
{
    NONE,
    PotatoSeeds
}

[CreateAssetMenu(fileName = "Item Data", menuName ="Item Data", order = 50)]
[System.Serializable]
public class ItemData : ScriptableObject
{
    public string itemName;
    public Sprite icon;
    public ItemType type;
}
