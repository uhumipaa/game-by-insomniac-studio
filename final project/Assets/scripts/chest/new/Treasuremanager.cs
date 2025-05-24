using UnityEngine;
using System.Collections.Generic;
public class Treasuremanager : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [SerializeField] RewardCards cards;

}
public enum ItemRarity
    {
        Common, Rare, Epic, Legendary
    };
[System.Serializable]
    public class RewardItem
    {
        public ItemData itemData;
        public ItemRarity rarity;
    }
[System.Serializable]
    public class extraRewardItem
    {
        public ItemData itemData;
        public int quantity;
    }
