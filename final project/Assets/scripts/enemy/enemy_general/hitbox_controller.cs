using UnityEngine;

public class Hitbox_Controller : MonoBehaviour
{
    private Player_Property player;
    private PolygonCollider2D col;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        /*
        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        Color c = sr.color;
        c.a = 0f;
        sr.color = c;
        */
        col = GetComponent<PolygonCollider2D>();
        col.enabled = false;
    }
    public void Enablecol()
    {
        if (col == null)
        {
            col = GetComponent<PolygonCollider2D>();
        }
        if (col == null)
        {
            Debug.LogError("Hitbox_Controller: 找不到 PolygonCollider2D");
            return;
        }
        /*
        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        Color c = sr.color;
        c.a = 1f;
        sr.color = c;
        */
        col.enabled = true;
    }
    public void Closecol()
    {
        /*
        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        Color c = sr.color;
        c.a = 0f;
        sr.color = c;
        */
        col.enabled = false;
    }
        private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            player = collision.GetComponent<Player_Property>();
            player.takedamage(player.atk,transform.position);

        }
    }

    // Update is called once per frame
}
