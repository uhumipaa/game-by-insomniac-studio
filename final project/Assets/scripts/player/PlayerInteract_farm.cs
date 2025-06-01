using UnityEngine;

public class PlayerInteract_farm : MonoBehaviour
{

    public ItemData water;
    public int waterAmount = 0; // 玩家擁有的水
    private bool isNearWell = false; // 是否靠近井

    void Update()
    {
        if (isNearWell && Input.GetKeyDown(KeyCode.T)) //按T鍵拿水
        {
            GetWater();
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Well"))
        {
            isNearWell = true;
            //Debug.Log("靠近井了，可以按 G 取水");
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Well"))
        {
            isNearWell = false;
           // Debug.Log("離開井了");
        }
    }

    void GetWater()
    {
        if (water == null)
        {
            water = Resources.Load<ItemData>("Items/water");
            if (water == null)
            {
                //Debug.LogError("❌ Resources/Items 資料夾中找不到名為 'water' 的 ItemData！");
                return;
            }
        }
        
        //每次獲得10滴水，並加入inventory
        InventoryManager.Instance.Add("Backpack", water, 10);
    }

}
