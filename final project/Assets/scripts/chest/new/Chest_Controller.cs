using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System.Collections;
public class Chest_Controller : MonoBehaviour
{

    public int Rate_Common;
    public int Rate_Rare;
    public int Rate_Epic;
    public int Rate_Lengendary;
    public List<RewardItem> rewardspool;
    //public List<extraRewardItem> extraRewards;
    //private List<extraRewardItem> currentextraRewards = new List<extraRewardItem>();
    public RewardItem presents_reward;
    public CanvasGroup rewardUI;
    public Animator ani;
    public RewardCards[] cards;
    private bool inrange;
    private bool isopen;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        ani = GetComponent<Animator>();
        cards = FindObjectsByType<RewardCards>(FindObjectsSortMode.None);
    }
    void Update()
    {
        if (inrange && !isopen && Input.GetKeyDown(KeyCode.Return))
        {
            ani.SetTrigger("open");
            StartCoroutine(opentreasure());
        }
        else if (isopen&&Input.GetKeyDown(KeyCode.Return))
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
        int lucky = PlayerStatusManager.instance.playerStatusData.Luck / 6;
        ItemRarity rarity=ItemRarity.Common;
        rmd += lucky;
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
        else if (Rate_Common + Rate_Rare + Rate_Epic <= rmd)
        {
            rarity =ItemRarity.Legendary;
        }
        return rarity;
    }
    IEnumerator opentreasure()
    {
        yield return new WaitForSecondsRealtime(1.5f);
        showreward();
    }
    RewardItem getrandomreward()
    {
        ItemRarity selectedRarity = GetRandomRarity();
        var filteredItems = rewardspool.Where(item => item.rarity == selectedRarity).ToList();
        return filteredItems[Random.Range(0, filteredItems.Count)];
    }
    /*
    void getextra()
    {
        currentextraRewards.Clear();
        for (int i = 0; i < 3; i++)
        {
            currentextraRewards.Add(extraRewards[Random.Range(0, extraRewards.Count)]);
        }
    }
    */

    public void showreward()
    {
        Debug.Log("123");
        for (int i = 0; i < 3; i++)
        {
            //getextra();
            //cards[i].populatecard(getrandomreward(), currentextraRewards);
            cards[i].populatecard(getrandomreward());
            cards[i].GetComponent<Animator>().SetTrigger("Move");
        }
        FindAnyObjectByType<Treasuremanager>().GetComponent<Treasuremanager>().openUI();

    }
}
