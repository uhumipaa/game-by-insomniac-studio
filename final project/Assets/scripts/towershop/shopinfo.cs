using UnityEngine;
using TMPro;
using System.Collections.Generic;
public class shopinfo : MonoBehaviour
{
    public CanvasGroup infopanel;
    public TMP_Text nametext;
    public TMP_Text descriptiontext;
    public TMP_Text[] effecttext;
    private RectTransform infopanelrect;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        infopanelrect = GetComponent<RectTransform>();
    }
    public void showinfo(ItemData item)
    {
        infopanel.alpha = 1;
        nametext.text = item.name;
        descriptiontext.text = item.description;
        List<string> stats = new List<string>();
        if (item.currentHP != 0) { stats.Add("Heal: " + item.currentHP); Debug.Log("→ 加入 Heal"); }
        if (item.buffATK != 0)   { stats.Add("ATK: " + item.buffATK);   Debug.Log("→ 加入 ATK"); }
        if (item.buffDEF != 0)   { stats.Add("DEF: " + item.buffDEF);   Debug.Log("→ 加入 DEF"); }
        if (item.buffMP != 0)    { stats.Add("MP: " + item.buffMP);     Debug.Log("→ 加入 MP"); }
        if (item.buffSP != 0)    { stats.Add("Speed: " + item.buffSP);  Debug.Log("→ 加入 Speed"); }
        if (item.duration != 0)  { stats.Add("Duration: " + item.duration + "s"); Debug.Log("→ 加入 Duration"); }
        if (item.maxHP != 0)     { stats.Add("Max HP: " + item.maxHP);  Debug.Log("→ 加入 Max HP"); }

        Debug.Log("stats count: " + stats.Count);
        Debug.Log($"Item: {item.name} ATK:{item.buffATK} DEF:{item.buffDEF} HP:{item.currentHP} MP:{item.buffMP}");
        if (stats.Count <= 0)
        {
            effecttext[0].text = new("Nothing happen");
            for (int i = 1; i < effecttext.Length; i++)
            {
                effecttext[i].text = "";
                effecttext[i].gameObject.SetActive(false);
            }
            return;
        }
        else
        {
            for (int i = 0; i < stats.Count; i++)
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
    public void hideinfo()
    {
        infopanel.alpha = 0;
    }
    public void followmouse()
    {
        Vector3 mouseposition = Input.mousePosition;
        Vector3 offset = new Vector3(50, -50, 0);
        infopanelrect.position = mouseposition + offset;
    }
}

