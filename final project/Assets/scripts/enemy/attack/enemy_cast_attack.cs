using UnityEngine;

public class enemy_cast_attack : MonoBehaviour,IEnemyAttackBehavior
{
    public GameObject[] cast_prefab;
    private int prefabcount;
    private float damage;
    IEnenmyResetInterface reset;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }
    public void  Attack(Transform self, Transform player, float attack, float scale)
    {
        if (self.position.x < player.position.x)
        {
            self.localScale = new Vector3(scale, scale, 1);
        }
        else
        {
            self.localScale = new Vector3(-scale, scale, 1);
        }
        Vector2 diff = player.position - transform.position;
        if (Mathf.Abs(diff.y) < 0.2f)
        {

            prefabcount = 0;
        }
        else
        {
            prefabcount = diff.y > 0 ? 1 : 2;
        }
        damage = attack;
    }

    public void enableprefab()
    {
        reset = cast_prefab[prefabcount].GetComponent<IEnenmyResetInterface>();
       
        reset.Reset();
        cast_prefab[prefabcount].SetActive(true);
        
    }
    public void closeprefab()
    {
        cast_prefab[prefabcount].SetActive(false);
    }
    // Update is called once per frame

}
