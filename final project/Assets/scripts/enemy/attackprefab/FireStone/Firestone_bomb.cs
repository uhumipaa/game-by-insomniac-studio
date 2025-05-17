using UnityEngine;

public class Firestone_bomb : MonoBehaviour
{
    [SerializeField] int damage;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            collision.GetComponent<Player_Property>().takedamage(damage, transform.position);
        }
    }
}
