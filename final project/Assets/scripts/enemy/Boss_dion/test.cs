using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
public class firestoneskill : MonoBehaviour,IEnemySpecilskillBehavior
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public GameObject firestone;
    [SerializeField] float rmd;
    private List<Vector2> haspos=new List<Vector2>();
    public void usingskill(Transform self, Transform player, enemy_property property, float scale)
    {
        haspos.Clear();
        int count=0;
        while (count <= 5)
        {
            Vector2 gene_pos=(Vector2)player.transform.position+new Vector2 (Random.Range(-rmd-0.1f,rmd), Random.Range(-rmd - 0.1f, rmd));
            if (haspos.Contains(gene_pos))
            {
                gene_pos = (Vector2)player.transform.position + new Vector2(Random.Range(-rmd - 0.1f, rmd), Random.Range(-rmd - 0.1f, rmd));
            }
            haspos.Add(gene_pos);

            Instantiate(firestone).GetComponent<FireStone_controller>().Setdrop(gene_pos);
            count++;
        }
        
    }
}
