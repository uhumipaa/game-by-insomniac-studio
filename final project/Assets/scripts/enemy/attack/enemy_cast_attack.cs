using UnityEngine;

public class enemy_cast_attack : MonoBehaviour,IEnemyAttackBehavior
{
    public GameObject cast_prefab;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }
    public void  Attack(Transform self, Transform player, float attack)
    {
        if (self.position.x < player.position.x)
        {
            self.localScale = new Vector3(1, 1, 1);
        }
        else
        {
            self.localScale = new Vector3(-1, 1, 1);
        }
    }

    public void enableprefab()
    {
        cast_prefab.SetActive(true);
    }
    public void closeprefab()
    {
        cast_prefab.SetActive(false);
    }
    // Update is called once per frame

}
