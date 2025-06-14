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
    Fishing_feel,
    Water,armor,helmet,boots,pants,accesasor,sword,staff,Atkpotion,Defpotion,Hppotion,Accpotion,potion
}

[CreateAssetMenu(fileName = "Item Data", menuName = "Item Data", order = 50)]
[System.Serializable]
public class ItemData : ScriptableObject
{
    public string ID;
    public string itemName;
    [TextArea] public string description;
    public Sprite icon;
    public ItemType type;
    public int maxAllowed = 100;
    public bool isusable;
    public bool isarmor;
    [Header("效果屬性")]
    public int currentHP;
    public int maxHP;
    public int buffATK;
    public int buffDEF;
    public float buffSP;
    public int buffMP;
    public int buffLuck;
    public float duration;

}

[System.Serializable]
public class SaveItem
{
    public string Name;
    public int quantity;
    public SaveItem(string name, int count)
    {
        Name = name;
        quantity = count;
    }
}
