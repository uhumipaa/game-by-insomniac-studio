using System.Collections.Generic;
using UnityEngine;

public class SelectionRewradsystem : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public RewardCard prefab;
    public Transform cardparent;
    private ChestInteraction chest;
    // Update is called once per frame
    public void show_card(RewardData rewardPool)
    {
        Instantiate(prefab, cardparent).GetComponent<RewardCard>().setupreward(rewardPool);
    }
    private void Start()
    {
        chest = FindAnyObjectByType<ChestInteraction>().GetComponent<ChestInteraction>();
    }
    private void Update()
    {
        if (chest.isOpened && transform.childCount == 0)
        {
            closeUI();
        }
    }
    public void closeUI()
    {
        gameObject.SetActive(false);
    }
}
