using UnityEngine;

public interface IEnemyMoveBehavior
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Move(Transform self, Transform player, Rigidbody2D rb, float speed);
}
