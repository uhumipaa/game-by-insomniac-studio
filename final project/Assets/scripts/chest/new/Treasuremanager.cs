using UnityEngine;
using System.Collections.Generic;
public class Treasuremanager : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [SerializeField] RewardCards cards;
    void Awake()
    {
        closeUI();       
    }
    public void closeUI()
    {
        CanvasGroup canvas = GetComponent<CanvasGroup>();
        canvas.alpha = 0;
        canvas.interactable = false;
        canvas.blocksRaycasts = false;
    }
    public void openUI()
    {
        CanvasGroup canvas = GetComponent<CanvasGroup>();
        canvas.alpha = 1;
        canvas.interactable = true;
        canvas.blocksRaycasts = true;
    }
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
