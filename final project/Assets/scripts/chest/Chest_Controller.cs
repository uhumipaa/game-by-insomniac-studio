using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;
public class Chest_Controller : MonoBehaviour
{

    public List<RewardData> r_rewards;
    public List<RewardData> sr_rewards;
    public List<RewardData> ssr_rewards;
    public List<RewardData> ur_rewards;
    public List<RewardData> actual_rewards;
    public GameObject presents_reward;
    public Transform generate_pos;
    public Animator ani;
    // Start is called once before the first execution of Update after the MonoBehaviour is created


    private void OnTriggerEnter2D(Collider2D collision)
    {
        
    }
    void ramdomreward()
    {
        int rmd = Random.Range(0, 100);
        if (rmd < 60)
        {
            RewardData reward = r_rewards[Random.Range(0, r_rewards.Count)];
            actual_rewards.Add(reward);
        }
        else if (60 <= rmd && rmd < 85)
        {
            RewardData reward = sr_rewards[Random.Range(0, r_rewards.Count)];
            actual_rewards.Add(reward);
        }
        else if (85 <= rmd && rmd < 95)
        {
            RewardData reward = ssr_rewards[Random.Range(0, r_rewards.Count)];
            actual_rewards.Add(reward);
        }
        else if (95 <= rmd && rmd < 100)
        {
            RewardData reward = ur_rewards[Random.Range(0, r_rewards.Count)];
            actual_rewards.Add(reward);
        }
    }
    public void showreward()
    {
        actual_rewards.Clear();
        for (int i = 0; i < 3; i++)
        {
            ramdomreward();
        }
        

    }
}
