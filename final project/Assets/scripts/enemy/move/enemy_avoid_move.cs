using System.Collections.Generic;
using UnityEngine;

public class enemy_avoid_move : MonoBehaviour,IEnemyMoveBehavior
{
    //玩家進入一定範圍會拉開距離
    //public float safeDistance;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public void Move(Transform self, Transform player, Rigidbody2D rb, float speed)
    {
            Vector2 awayDir = (self.position - player.position).normalized;
            if (self.position.x > player.position.x)
            {
                transform.localScale = new Vector3(1, 1, 1);
            }
            else
            {
                transform.localScale = new Vector3(-1, 1, 1);
            }
            rb.linearVelocity = awayDir * speed;
        
    }
}
