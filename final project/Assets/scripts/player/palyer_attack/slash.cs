using System.Collections;
using UnityEngine;

public class slash : MonoBehaviour
{
    private Player_Property player;
    private Transform player_transform;
    private enemy_property enemy;
    private BoxCollider2D col;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        col = GetComponent<BoxCollider2D>();
        col.enabled = false;
        player = GameObject.FindGameObjectWithTag("Player")?.GetComponent<Player_Property>();
        player_transform = GameObject.FindGameObjectWithTag("Player")?.transform;
    }
    // Update is called once per frame
    public IEnumerator HitboxRoutine(float t)
    {
        col.enabled = true;
        yield return new WaitForSeconds(t);
        col.enabled = false;
        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("enemy"))
        {
            enemy = collision.GetComponent<enemy_property>();
            if (enemy == null)
            {
            }
            enemy.takedamage(player.atk, player_transform.position);

        }
    }
}
