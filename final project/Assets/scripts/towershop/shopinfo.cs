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
        if (item.currentHP != 0) stats.Add("heal: " + item.currentHP.ToString());
        if (item.bufDEF != 0) stats.Add("ATK: " + item.buffATK.ToString());
        if (item.buffMP != 0) stats.Add("MP: " + item.buffMP.ToString());
        if (item.buffSP != 0) stats.Add("Speed: " + item.buffSP.ToString());
        if (item.duration != 0) stats.Add("duration: " + item.duration.ToString()+"s");
        if (item.maxHP != 0) stats.Add("maxHP: " + item.maxHP.ToString());
        if (stats.Count <= 0)
        {
            effecttext[0].text = new("Nothing happen");
            for (int i = 1; i < effecttext.Length; i++){
                effecttext[i].text = "";
                effecttext[i].gameObject.SetActive(false);
            }
            return;
        }
        for (int i = 0; i < stats.Count; i++) {
            effecttext[i].text = stats[i];
            effecttext[i].gameObject.SetActive(true);
        }
        for (int i = stats.Count; i < effecttext.Length; i++) {
            effecttext[i].text = "";
            effecttext[i].gameObject.SetActive(false);
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

