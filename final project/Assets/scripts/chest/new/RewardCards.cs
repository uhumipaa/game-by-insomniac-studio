using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections.Generic;
public class RewardCards : MonoBehaviour
{
    [SerializeField] TMP_Text nametext;
    [SerializeField] TMP_Text[] effecttext;
    [SerializeField] TMP_Text[] extratext;
    [SerializeField] Image icon;
    private ItemData item;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {

    }
    public void populatecard(RewardItem newrewardItem,List<extraRewardItem> extra)
    {
        item = newrewardItem.itemData;
        nametext.text = item.name;
        icon.sprite = item.icon;
        for (int i = 0; i < extra.Count && i < extratext.Length; i++)
        {
            extratext[i].text = new(extra[i].itemData.name + "x" + extra[i].quantity);
        }
        List<string> stats = new List<string>();
        if (item.currentHP != 0) { stats.Add("Heal: " + item.currentHP); Debug.Log("→ 加入 Heal"); }
        if (item.buffATK != 0)   { stats.Add("ATK: " + item.buffATK);   Debug.Log("→ 加入 ATK"); }
        if (item.buffDEF != 0)   { stats.Add("DEF: " + item.buffDEF);   Debug.Log("→ 加入 DEF"); }
        if (item.buffMP != 0)    { stats.Add("MP: " + item.buffMP);     Debug.Log("→ 加入 MP"); }
        if (item.buffSP != 0)    { stats.Add("Speed: " + item.buffSP);  Debug.Log("→ 加入 Speed"); }
        if (item.duration != 0)  { stats.Add("Duration: " + item.duration + "s"); Debug.Log("→ 加入 Duration"); }
        if (item.maxHP != 0)     { stats.Add("Max HP: " + item.maxHP);  Debug.Log("→ 加入 Max HP"); }
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
            for (int i = 0; i < stats.Count&& i < effecttext.Length; i++)
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
}
