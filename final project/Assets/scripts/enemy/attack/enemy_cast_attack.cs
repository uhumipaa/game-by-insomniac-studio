using UnityEngine;

public class enemy_cast_attack : MonoBehaviour,IEnemyAttackBehavior
{
    public GameObject[] cast_prefab;
    private int prefabcount;
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
        if (Mathf.Abs(diff.y) < 1f)
        {

            prefabcount = 0;
        }
        else
        {
            prefabcount = diff.y > 0 ? 1 : 2;
        }
    }

    public void enableprefab()
    {
        reset = cast_prefab[prefabcount].GetComponent<IEnenmyResetInterface>();
        if (reset != null)
        {
            cast_prefab[prefabcount].SetActive(true);
            reset.Reset(); // Reset ­t³dªì©l¤Æ & ±Ò°Ê
        }
        else
        {
            cast_prefab[prefabcount].SetActive(true); // fallback
        }
        Debug.Log("reset");
    }
    public void closeprefab()
    {
        cast_prefab[prefabcount].SetActive(false);
    }
    // Update is called once per frame

}
