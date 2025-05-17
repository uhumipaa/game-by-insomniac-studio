using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;
public class Enemy_8DireactionCast_Attack : MonoBehaviour,IEnemyAttackBehavior
{
    public List<GameObject> hitboxes = new List<GameObject>();
    IEnenmyResetInterface reset;
    private GameObject currenthitbox;
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    // Update is called once per frame
    public void Attack(Transform self, Transform player, float attack, float scale)
    {
        if (hitboxes == null || hitboxes.Count == 0 || player == null)
            return;

        float minDistance = float.MaxValue;

        foreach (GameObject hitbox in hitboxes)
        {
            float distance = Vector2.Distance(hitbox.transform.position, player.position);
            if (distance < minDistance)
            {
                minDistance = distance;
                currenthitbox = hitbox;
            }
            
        }
        
        foreach (GameObject hitbox in hitboxes)
            hitbox.SetActive(false); // 先全部關掉
    }
    public void enableprefab()
    {
        reset = currenthitbox.GetComponent<IEnenmyResetInterface>();
        if (reset != null)
        {
            currenthitbox.SetActive(true);
            reset.Reset(); // Reset 負責初始化 & 啟動
        }
        else
        {
            currenthitbox.SetActive(true); // fallback
        }
        Debug.Log("reset");
    }
    public void closeprefab()
    {
        currenthitbox.SetActive(false);
    }
}
