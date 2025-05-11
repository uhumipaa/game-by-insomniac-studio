using Unity.VisualScripting.Antlr3.Runtime.Tree;
using UnityEngine;

public class enemy_throw_attack : MonoBehaviour,IEnemyAttackBehavior
{
    Transform Player;
    Vector2 direction;
    public GameObject prefab;
    private Transform Self;
    private float dmg;
    public void Attack(Transform self, Transform player, float attack)
    {
        if (self.position.x < player.position.x)
        {
            self.localScale = new Vector3(1, 1, 1);
        }
        else
        {
            self.localScale = new Vector3(-1, 1, 1);
        }
        direction = (player.position - self.position).normalized;
        Player = player;
        Self = self;
        dmg = attack;
    }
    public void Throwthing()
    {
        GameObject throw_thing = Instantiate(prefab, Self.position, Quaternion.identity);
        throw_thing.transform.position = Self.position;
        var controller = throw_thing.GetComponent<throw_thing_controller>();
        controller.Set_parabola((Vector2)Player.position,direction,dmg);
    }
    
}
