using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;
public class Chest_Controller : MonoBehaviour
{

    public List<RewardData> r_rewards;
    public List<RewardData> sr_rewards;
    public List<RewardData> ssr_rewards;
    public List<RewardData> ur_rewards;
    public RewardData presents_reward;
    public GameObject rewardUI;
    public Animator ani;
    private SelectionRewradsystem rewradsystem;
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    private void Start()
    {
        rewradsystem =rewardUI.GetComponent<SelectionRewradsystem>();
    }
    void ramdomreward()
    {
        int rmd = Random.Range(0, 100);
        if (rmd < 60)
        {
            presents_reward = r_rewards[Random.Range(0, r_rewards.Count)];
        }
        else if (60 <= rmd && rmd < 85)
        {
            presents_reward = sr_rewards[Random.Range(0, sr_rewards.Count)];
        }
        else if (85 <= rmd && rmd < 95)
        {
            presents_reward = ssr_rewards[Random.Range(0, ssr_rewards.Count)];
        }
        else if (95 <= rmd && rmd < 100)
        {
            presents_reward = ur_rewards[Random.Range(0, ur_rewards.Count)];
        }
    }
    public void showreward()
    {
        rewardUI.SetActive(true);
        for (int i = 0; i < 3; i++)
        {
            ramdomreward();
            rewradsystem.show_card(presents_reward);
        }
        

    }
}
