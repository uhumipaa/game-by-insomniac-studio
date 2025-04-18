using System.Collections.Generic;
using UnityEngine;

public class Inventory_UI : MonoBehaviour
{
    public GameObject inventoryPanel;
    //public Player player;
    public List<Slot_UI> slots = new List<Slot_UI>();
    void Start()
    {
        // 一開始關閉背包 UI
        inventoryPanel.SetActive(false);
    }
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Tab))
        {
            ToggleInventory();
        }
    }

    public void ToggleInventory()
    {
        if(!inventoryPanel.activeSelf)
        {
            inventoryPanel.SetActive(true);
            //Setup();
        }
        else
        {
            inventoryPanel.SetActive(false);
        }
    }

    /*void Setup(){
        if(slots.Count == player.inventory.slots.Count)
        {
            for(int i = 0; i < slots.Count; i++)
            {
                if 
            }
        }
    }*/
}
