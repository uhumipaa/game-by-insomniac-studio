using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine.EventSystems;
public class RewardCards : MonoBehaviour,IPointerClickHandler
{
    [SerializeField] CanvasGroup rewardUI;
    [SerializeField] TMP_Text nametext;
    [SerializeField] TMP_Text[] effecttext;
    [SerializeField] TMP_Text cointext;
    [SerializeField] Image namebanner;
    [SerializeField] Image icon;
    private ItemData item;
    int coin;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public void populatecard(RewardItem newrewardItem)
    {
        item = newrewardItem.itemData;
        nametext.text = item.name;
        icon.sprite = item.icon;
        switch (newrewardItem.rarity)
        {
            case ItemRarity.Common:
                namebanner.color = Color.green;
                break;
            case ItemRarity.Rare:
                namebanner.color = Color.blue;
                break;
            case ItemRarity.Epic:
                namebanner.color = new Color(1f, 0f, 1f);
                break;
            case ItemRarity.Legendary:
                namebanner.color = Color.red;
                break;
        }
        coin = Random.Range(1, 11) * 10;
        cointext.text = new("Coin x" + coin);
        List<string> stats = new List<string>();
        if (item.currentHP != 0) { stats.Add("Heal: " + item.currentHP); Debug.Log("→ 加入 Heal"); }
        if (item.buffATK != 0) { stats.Add("ATK: " + item.buffATK); Debug.Log("→ 加入 ATK"); }
        if (item.buffDEF != 0) { stats.Add("DEF: " + item.buffDEF); Debug.Log("→ 加入 DEF"); }
        if (item.buffMP != 0) { stats.Add("MP: " + item.buffMP); Debug.Log("→ 加入 MP"); }
        if (item.buffSP != 0) { stats.Add("Speed: " + item.buffSP); Debug.Log("→ 加入 Speed"); }
        if (item.duration != 0) { stats.Add("Duration: " + item.duration + "s"); Debug.Log("→ 加入 Duration"); }
        if (item.maxHP != 0) { stats.Add("Max HP: " + item.maxHP); Debug.Log("→ 加入 Max HP"); }
        if (stats.Count <= 0)
        {
            effecttext[6].text = new("Maybe it can be use on other condition");
            effecttext[6].gameObject.SetActive(true);
            for (int i = 0; i < effecttext.Length - 1; i++)
            {
                effecttext[i].text = "";
                effecttext[i].gameObject.SetActive(false);
            }
            return;
        }
        else
        {
            for (int i = 0; i < stats.Count && i < effecttext.Length; i++)
            {
                effecttext[i].text = stats[i];
                effecttext[i].gameObject.SetActive(true);
            }
            for (int i = stats.Count; i < effecttext.Length; i++)
            {
                effecttext[i].text = "";
                effecttext[i].gameObject.SetActive(false);
            }
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (rewardUI == null)
        {
            rewardUI = FindAnyObjectByType<Treasuremanager>().GetComponent<CanvasGroup>();
        }
        FindAnyObjectByType<Treasuremanager>().GetComponent<Treasuremanager>().closeUI();
        InventoryManager.Instance.Add("Backpack", item, 1);
        CoinManager.instance.AddCoins(coin);
    }

}
