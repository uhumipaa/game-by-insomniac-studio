using System.Collections.Generic;

using UnityEngine;


public class Getclosestenemy : MonoBehaviour
{
    public LayerMask enemylayer;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    
    public GameObject getclosest(Vector3 position,List<GameObject>exceptenemy,float searchradius)
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, searchradius, enemylayer);
        GameObject closestenemy = null;
        float closestdistance = Mathf.Infinity;
        foreach (Collider2D hit in hits)
        {
            GameObject enemy = hit.gameObject;
            if (exceptenemy.Contains(enemy)) continue;
            float newdistance = Vector3.Distance(enemy.transform.position , position);
            if (newdistance < closestdistance && newdistance != closestdistance)
            {   
                closestdistance = newdistance;
                closestenemy = enemy;
            }
        }
        return closestenemy;
    }

    // Update is called once per frame
}
