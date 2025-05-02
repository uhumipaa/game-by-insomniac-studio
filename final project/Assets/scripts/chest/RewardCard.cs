using UnityEngine;
using TMPro;
using UnityEngine.UI;
using TMPro.Examples;
using System.Collections.Generic;
public class RewardCard : MonoBehaviour
{
    public Image icon;
    public Image type;
    public TextMeshProUGUI description;
    public TextMeshProUGUI Name;
    public TextMeshProUGUI extrareward; 
    private RewardData data;
    public List<ExtraRewardData> otherpool;
    public List<ExtraRewardData> present_extra;
    public GameObject UI;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public void setupreward(RewardData reward)
    {
        data = reward;
        icon.sprite = data.icon;
        type.sprite = data.type;
        description.text = data.statement;
        Name.text = data.rewardname;
        present_extra = new List<ExtraRewardData>();
        for (int i =  0; i < 5; i++)
        {
            if (i>1&&Random.value<0.66f)
            {
                present_extra.Add(otherpool[Random.Range(0, otherpool.Count)]);
            }
        }
        setextra();
    }
    void setextra()
    {
        extrareward.text = "";
        foreach (var e in present_extra)
        {
            extrareward.text += $"+{e.amount} {e.extra_name}\n";
        }
    }
    // Update is called once per frame
    public void OnClick()
    {
        Debug.Log("點到了卡片！");
        data.ApplyReward(GameObject.FindWithTag("Player"));
        foreach (var e in present_extra)
        {
            e.ApplyExtra(GameObject.FindWithTag("Player"));
        }
        Destroy(transform.parent.gameObject);
    }
}
