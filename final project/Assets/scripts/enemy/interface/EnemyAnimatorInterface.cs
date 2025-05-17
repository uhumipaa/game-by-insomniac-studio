using Unity.VisualScripting;
using UnityEngine;

public interface IEnemyAnimatorBehavior 
{
    void PlayAttack(Vector2 direction,Animator ani);
    void PlayMove(Vector2 direction,Animator ani);
    void PlayIdle(Animator ani);
    void NotifyAttackFinished();
}
