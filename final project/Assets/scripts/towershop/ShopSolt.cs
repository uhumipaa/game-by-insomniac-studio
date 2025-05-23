using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Shopsolt : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerMoveHandler
{
    public ItemData item;
    public TMP_Text nametext;
    public TMP_Text pricetext;
    public Image icon;
    private int price;
    [SerializeField] shopmanager shopmanager;
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
        shopmanager.trytobuyitem(item, price);
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
}
