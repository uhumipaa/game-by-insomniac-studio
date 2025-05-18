using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine.PlayerLoop;
using Unity.VisualScripting;
public class Sommonfirestoneskill : MonoBehaviour, IEnemySpecilskillBehavior
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public GameObject firestone;
    [SerializeField] float rmd;
    [SerializeField] float summontime;
    private List<Vector2> haspos = new List<Vector2>();
    private IEnemySkillContollerInterface controller;
    private void Start()
    {
        controller = GetComponent<IEnemySkillContollerInterface>();
    }

    private IEnumerator sommon(Transform player)
    {

        haspos.Clear();
        int count = 0;
        while (count <= 5)
        {
            Vector2 gene_pos = (Vector2)player.transform.position + new Vector2(Random.Range(-rmd - 0.1f, rmd), Random.Range(-rmd - 0.1f, rmd));
            if (haspos.Contains(gene_pos))
            {
                gene_pos = (Vector2)player.transform.position + new Vector2(Random.Range(-rmd - 0.1f, rmd), Random.Range(-rmd - 0.1f, rmd));
            }
            haspos.Add(gene_pos);
            Instantiate(firestone).GetComponent<FireStone_controller>().Setdrop(gene_pos);
            yield return new WaitForSecondsRealtime(summontime);
            count++;
        }
        controller.Finishskill();
        
    }

    public void usingskill(Transform self, Transform player, enemy_property property, float scale)
    {
        StartCoroutine(sommon(player));
    }
}
