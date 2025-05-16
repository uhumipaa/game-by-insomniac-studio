using UnityEngine;

public interface IEnemyAttackBehavior
{
    void Attack(Transform self, Transform player,float attack,float scale);
}
