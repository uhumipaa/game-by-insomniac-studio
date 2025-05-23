using UnityEngine;

public class PlayerInteract_farm : MonoBehaviour
{
    
    public ItemManager itemManager;
    public int waterAmount = 0; // 玩家擁有的水
    private bool isNearWell = false; // 是否靠近井

    void Update()
    {
        if (isNearWell && Input.GetKeyDown(KeyCode.G))
        {
            GetWater();
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Well"))
        {
            isNearWell = true;
            Debug.Log("靠近井了，可以按 G 取水");
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Well"))
        {
            isNearWell = false;
            Debug.Log("離開井了");
        }
    }

    void GetWater()
    {
        ItemData waterData = itemManager.GetItemData(ItemType.Water);

        //每次獲得10滴水，並加入inventory
        InventoryManager.Instance.Add("Backpack", waterData, 1);

    }

}
