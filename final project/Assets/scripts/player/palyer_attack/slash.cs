using System.Collections;
using UnityEngine;

public class slash : MonoBehaviour
{
    private Player_Property player;
    private Transform plater_transform;
    private enemy_property enemy;
    private BoxCollider2D col;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
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
        Debug.Log("ï¿½}ï¿½ï¿½");
        col.enabled = true;
        yield return new WaitForSeconds(t);
        col.enabled = false;
<<<<<<< Updated upstream
        Debug.Log("ï¿½ï¿½ï¿½ï¿½");
=======
        Destroy(gameObject);
        Debug.Log("Ãö³¬");
>>>>>>> Stashed changes
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("enemy"))
        {
            enemy = collision.GetComponent<enemy_property>();
            enemy.takedamage(player.atk, plater_transform.position);

        }
    }
}
