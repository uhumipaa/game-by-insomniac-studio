using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

public class TowerBagUI : MonoBehaviour
{
    public static TowerBagUI instance;
    public GameObject slotPrefab;
    public Transform slotParent;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Update is called once per frame
    public void Refresh()
    {
        foreach (Transform child in slotParent)
        {
            Destroy(child.gameObject);
        }

        foreach (var item in BagSystemForTower.Instance.items)
        {
            GameObject slot = Instantiate(slotPrefab, slotParent);
            Image icon = slot.transform.Find("Image").GetComponent<Image>();
            TextMeshProUGUI qtyText = slot.transform.Find("QuantityText").GetComponent<TextMeshProUGUI>();
            if (icon == null||qtyText==null)
            {
                Debug.Log("aaaaa");
            }
            Debug.Log("aaaaa");
            icon.sprite = item.itemData.icon;
            qtyText.text = $"x{item.quantity}";
            Debug.Log("aaaaa");
        }
    }
}
