using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

public enum ItemType
{
    NONE,
    PotatoSeeds, Potato,
    RadishSeeds, Radish,
    TurnipSeeds, Turnip,
    SpinachSeeds, Spinach,
    Magikarp, Starfish,
    Gyarados, Guppy,
    Golden_Tench, Nemo,
    Plastic_Bag, Message_In_A_Bottle,
    Gundam, Coins,
    Water
}

[CreateAssetMenu(fileName = "Item Data", menuName = "Item Data", order = 50)]
[System.Serializable]
public class ItemData : ScriptableObject
{
    public string itemName;
    [TextArea] public string description;
    public Sprite icon;
    public ItemType type;
    public int maxAllowed = 99;
    public bool isusable;
    public bool isarmor;
    [Header("效果屬性")]
    public int currentHP;
    public int maxHP;
    public int buffATK;
    public int buffDEF;
    public float buffSP;
    public int buffMP;
    public float duration;
    
}
