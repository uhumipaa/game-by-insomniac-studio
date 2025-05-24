using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;
public class Chest_Controller : MonoBehaviour
{

    public int Rate_Common;
    public int Rate_Rare;
    public int Rate_Epic;
    public int Rate_Lengendary;
    public List<RewardItem> rewardspool;
    public List<extraRewardItem> extraRewards;
    private List<extraRewardItem> currentextraRewards = new List<extraRewardItem>();
    public RewardItem presents_reward;
    public CanvasGroup rewardUI;
    public Animator ani;
    public RewardCards[] cards;
    private bool inrange;
    private bool isopen;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        cards = FindObjectsByType<RewardCards>(FindObjectsSortMode.None);
        rewardUI = FindAnyObjectByType<Treasuremanager>().GetComponent<CanvasGroup>();      
    }
    void Update()
    {
        if (inrange && !isopen && Input.GetKeyDown(KeyCode.Return))
        {
            showreward();
        }
        else if(Input.GetKeyDown(KeyCode.Return))
        {
            rewardUI.alpha = 0;
        }
   }
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            inrange = true;
        }       
    }
    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            inrange = false;
        }  
    }

    ItemRarity GetRandomRarity()
    {
        int rmd = Random.Range(0, 100);
        ItemRarity rarity=ItemRarity.Common;
        if (rmd < Rate_Common)
        {
            rarity = ItemRarity.Common;
        }
        else if (Rate_Common <= rmd && rmd < Rate_Common + Rate_Rare)
        {
            rarity = ItemRarity.Rare;
        }
        else if (Rate_Common + Rate_Rare <= rmd && rmd < Rate_Common + Rate_Rare + Rate_Epic)
        {
            rarity =ItemRarity.Epic;
        }
        else if (Rate_Common + Rate_Rare + Rate_Epic <= rmd && rmd < Rate_Common + Rate_Rare + Rate_Epic + Rate_Lengendary)
        {
            rarity =ItemRarity.Legendary;
        }
        return rarity;
    }
    RewardItem getrandomreward()
    {
        ItemRarity selectedRarity = GetRandomRarity();
        var filteredItems = rewardspool.Where(item => item.rarity == selectedRarity).ToList();
        return filteredItems[Random.Range(0, filteredItems.Count)];
    }
    void getextra()
    {
        currentextraRewards.Clear();
        for (int i = 0; i < 3; i++)
        {
            currentextraRewards.Add(extraRewards[Random.Range(0, extraRewards.Count)]);
        }
    }
    public void showreward()
    {
        rewardUI.alpha = 1;
        for (int i = 0; i < 3; i++)
        {
            getextra();
            cards[i].populatecard(getrandomreward(), currentextraRewards);
            cards[i].GetComponent<Animator>().SetTrigger("Move");
        }

    }
}
