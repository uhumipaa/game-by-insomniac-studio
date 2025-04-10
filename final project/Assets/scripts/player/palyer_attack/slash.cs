using System.Collections;
using UnityEngine;

public class slash : MonoBehaviour
{
    private Player_Property player;
    private Transform plater_transform;
    private enemy_property enemy;
    private BoxCollider2D col;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        col = GetComponent<BoxCollider2D>();
        col.enabled = false;
        player = GameObject.FindGameObjectWithTag("Player")?.GetComponent<Player_Property>();
        plater_transform = GameObject.FindGameObjectWithTag("Player")?.transform;
    }

    public void enable_hitbox(float duration)
    {
        StartCoroutine(HitboxRoutine(duration));
    }
    // Update is called once per frame
    public IEnumerator HitboxRoutine(float t)
    {
        Debug.Log("¶}±Ò");
        col.enabled = true;
        yield return new WaitForSeconds(t);
        col.enabled = false;
        Debug.Log("Ãö³¬");
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("enemy"))
        {
            enemy = collision.GetComponent<enemy_property>();
            enemy.takedamage(player.atk);

        }
    }
}
