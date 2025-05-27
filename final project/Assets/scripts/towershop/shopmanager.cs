using System;
using System.Collections.Generic;
using UnityEngine;

public class shopmanager : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    [SerializeField] private Shopsolt[] shopsolts;


    void Start()
    {

    }
    public void populateshopitem(List<Shopitems> shopitems)
    {
        for (int i = 0; i < shopitems.Count && i < shopsolts.Length; i++)
        {
            Shopitems items = shopitems[i];
            shopsolts[i].initailize(items.itemdata, items.price);
            shopsolts[i].gameObject.SetActive(true);
        }
        for (int i = shopitems.Count; i < shopsolts.Length; i++)
        {
            shopsolts[i].gameObject.SetActive(false);
        }
    }
    public void trytobuyitem(ItemData item, int price)
    {

    }
    
}
[System.Serializable]
public class Shopitems
{
    public ItemData itemdata;
    public int price;
}
