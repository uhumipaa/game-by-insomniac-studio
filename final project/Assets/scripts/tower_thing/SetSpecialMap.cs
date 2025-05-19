using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using UnityEngine;

public class SetSpecialMap : MonoBehaviour
{
    public GameObject uhumipa;
    private FloorData bossdata;
    public List<ItemData> itemdatas = new List<ItemData>();

    private List<ItemData> goods = new List<ItemData>();

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public void setrest()
    {
        Instantiate(uhumipa, Vector3.zero, Quaternion.identity);
        goods.Clear();
        while (true)
        {
            ItemData data = itemdatas[Random.Range(0, itemdatas.Count)];
            if (goods.Contains(data)) {
                continue;
            }
            goods.Add(data);
            if (goods.Count >= 4) break;
        }
    }
    public void setboss(FloorData data)
    {
        bossdata = data;
        int level = TowerManager.Instance.currentTowerFloor;
        Instantiate(data.enemyprefab[0].enemyprefab, Vector3.zero, Quaternion.identity).GetComponent<enemy_property>()?.generaterandomstatus(data.enemyprefab[0], level);
    }
}
