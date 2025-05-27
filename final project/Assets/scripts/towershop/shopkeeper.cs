using UnityEngine;
using System;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using System.Linq;
public class shopkeeper : MonoBehaviour
{
    public CanvasGroup shop;
    public Animator hintani;
    private bool isshopopen;
    private bool inrange;
    public Shopmanager Shopmanager;
    [SerializeField] private List<Shopitems> shopitemspool;
    [SerializeField] private List<Shopitems> shopitems;
    [SerializeField] private List<Shopitems> shopposions;
    [SerializeField] private List<Shopitems> shopsurprisepool;
    [SerializeField] private List<Shopitems> shopsurprise;
    public static event Action<Shopmanager, bool> onshopstatechanged;
    public Loot[] surprices;
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            inrange = true;
            hintani.SetBool("inrange", true);
        }
    }
    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            inrange = false;
            hintani.SetBool("inrange", false);
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
                openitems();
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
        List<Shopitems> shuffled = new List<Shopitems>(shopsurprisepool);
        for (int i = 0; i < shuffled.Count; i++)
        {
            int randIndex = UnityEngine.Random.Range(i, shuffled.Count);
            (shuffled[i], shuffled[randIndex]) = (shuffled[randIndex], shuffled[i]);
        }
        shopsurprise = shuffled.Take(Mathf.Min(shuffled.Count,4)).OrderBy(item => item.price).ToList();
        for (int i = 0; i < 4; i++)
        {
            surprices[i].itemdata = shopsurprise[i].itemdata;
            surprices[i].updateloot();
        }
        /*
        int i = 0;
        while (true)
        {

            Shopitems data = shopsurprisepool[UnityEngine.Random.Range(0, shopsurprisepool.Count)];
            if (shopsurprise.Contains(data))
            {
                continue;
            }
            shopsurprise.Add(data);
            surprices[i].itemdata = data.itemdata;
            surprices[i].updateloot();
            i++;
            if (shopsurprise.Count >= 4) break;
        }
        */
    }
    public void setitem()
    {
        shopitems.Clear();
        List<Shopitems> shuffled = new List<Shopitems>(shopitemspool);
        for (int i = 0; i < shuffled.Count; i++)
        {
            int randIndex = UnityEngine.Random.Range(i, shuffled.Count);
            (shuffled[i], shuffled[randIndex]) = (shuffled[randIndex], shuffled[i]);
        }
        shopitems = shuffled.Take(Mathf.Min(shuffled.Count,8)).OrderBy(item => item.price).ToList();
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
    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
        setsurprice();
        setitem();
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        setsurprice();
        setitem();
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created

}
