using UnityEngine;
using System;
using System.Collections.Generic;
public class shopkeeper : MonoBehaviour
{
    public CanvasGroup shop;
    public Animator hintani;
    private bool isshopopen;
    private bool inrange;
    public shopmanager Shopmanager;
    [SerializeField] private List<Shopitems> shopitems;
    [SerializeField] private List<Shopitems> shopposions;
    [SerializeField] private List<Shopitems> shopsurprisepool;
    [SerializeField] private List<Shopitems> shopsurprise;
    public static event Action<shopmanager, bool> onshopstatechanged;
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Palyer"))
        {
            inrange = true;
            hintani.SetBool("inrange", true);
        }
    }
    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Palyer"))
        {
            inrange = false;
            hintani.SetBool("inrange", true);
        }
    }
    void Update()
    {
        if (inrange && Input.GetKeyDown(KeyCode.Return))
        {
            if (!isshopopen)
            {
                onshopstatechanged?.Invoke(Shopmanager, true);
                Time.timeScale = 0;
                shop.alpha = 1;
                shop.interactable = true;
                shop.blocksRaycasts = true;
                isshopopen = true;
            }
            else
            {
                Time.timeScale = 1;
                onshopstatechanged?.Invoke(Shopmanager, false);
                shop.alpha = 0;
                shop.interactable = false;
                shop.blocksRaycasts = false;
                isshopopen = false;
            }
        }
    }
    public void setsurprice()
    {
        shopsurprise.Clear();
        while (true)
        {
            Shopitems data = shopsurprisepool[UnityEngine.Random.Range(0, shopsurprisepool.Count)];
            if (shopsurprise.Contains(data))
            {
                continue;
            }
            shopsurprise.Add(data);
            if (shopsurprise.Count >= 4) break;
        }
    }
    public void openitems()
    {
        Shopmanager.populateshopitem(shopitems);
    }
    public void openposions()
    {
        Shopmanager.populateshopitem(shopposions);
    }
    public void opensurprise()
    {
        Shopmanager.populateshopitem(shopsurprise);
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created

}
