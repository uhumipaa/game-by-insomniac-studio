using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Slot_UI : MonoBehaviour
{
    public int slotID;
    public Inventory inventory;
    public Image itemIcon;
    public TextMeshProUGUI quantityText;

    [SerializeField] private GameObject highlight;

    public void SetItem(Inventory.Slot slot, Item item)
    {
        Debug.Log($"SetItem() → icon: {item?.data?.icon?.name}, count: {slot.count}");
        if(slot != null)
        {
            itemIcon.sprite = item.data.icon;
            Debug.Log("設定圖片為：" + slot.icon?.name);

            itemIcon.color = new Color(1,1,1,1);
            quantityText.text = slot.count.ToString();
        }
    }

    public void SetEmpty()
    {
        itemIcon.sprite = null;
        itemIcon.color = new Color(1, 1, 1, 0);
        quantityText.text = "";
    }

    public void SetHighlight(bool isOn)
    {
        highlight.SetActive(isOn);

    }
}
