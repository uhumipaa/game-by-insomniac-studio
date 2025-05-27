using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;

public class Shopsolt : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerMoveHandler
{
    public ItemData item;
    public TMP_Text nametext;
    public TMP_Text pricetext;
    public Image icon;
    private int price;
    [SerializeField] GameObject cantbuyhint;
    [SerializeField] Shopmanager shopmanager;
    [SerializeField] shopinfo info;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public void initailize(ItemData newitem, int price)
    {
        item = newitem;
        nametext.text = item.itemName;
        icon.sprite = item.icon;
        this.price = price;
        pricetext.text = price.ToString();
    }
    public void OnButtonClick()
    {
        if (!shopmanager.cantbuytemp)
        {
            if (CoinManager.instance.SpendCoins(price))
        {
            InventoryManager.Instance.Add("Backpack", item, 1);
        } 
        else
        {
                showcantbuyitem();
        }
        }
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (item != null)
        {
            info.showinfo(item);
        }

    }
    public void OnPointerExit(PointerEventData eventData)
    {
        info.hideinfo();
    }
    public void OnPointerMove(PointerEventData eventData)
    {
        if (item != null)
        {
            info.followmouse();
        }
    }
    IEnumerator showcantbuyitem()
    {
        cantbuyhint.SetActive(true);
        shopmanager.cantbuytemp = true;
        yield return new WaitForSecondsRealtime(0.5f);
        cantbuyhint.SetActive(false);
        shopmanager.cantbuytemp = false;
    }
}
