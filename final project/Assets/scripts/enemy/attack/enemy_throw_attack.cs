using Unity.VisualScripting.Antlr3.Runtime.Tree;
using UnityEngine;

public class enemy_throw_attack : MonoBehaviour,IEnemyAttackBehavior
{
    Transform Player;
    Vector2 direction;
    public GameObject prefab;
    public void Attack(Transform self, Transform player, float attack)
    {
        if (self.position.x < player.position.x)
        {
            transform.localScale = new Vector3(1, 1, 1);
        }
        else
        {
            transform.localScale = new Vector3(-1, 1, 1);
        }
        direction = (player.position - self.position).normalized;
        Player = player;
    }
    public void Throwthing()
    {
        GameObject throw_thing = Instantiate(prefab, transform.position, Quaternion.identity);
        throw_thing.GetComponent<Rigidbody2D>().linearVelocity = direction * 5f;
        throw_thing.GetComponent<throw_thing_controller>().Set_parabola((Vector2)Player.position);
    }
    
}
