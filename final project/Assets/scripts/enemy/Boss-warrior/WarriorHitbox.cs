using UnityEngine;

public class WarriorHitbox : MonoBehaviour
{
    private Collider2D hitboxCollider;
    public enemy_property enemy;

    private void Awake()
    {
        hitboxCollider = GetComponent<Collider2D>();
        if (hitboxCollider != null)
            hitboxCollider.enabled = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            // �ˮ`�B�z�]�I�s player_property�^
            var property = collision.GetComponent<Player_Property>();
            if (property != null)
            {
                property.takedamage(enemy.atk, transform.position); // ���T�G�ŦX��k�ѼƻP�R�W

            }

            // ���h�B�z�]�I�s Knockback�^
            var knock = collision.GetComponent<Knockback>();
            if (knock != null)
            {
                knock.ApplyKnockback(transform.position); // �ǤJ Boss ����m
            }

            Debug.Log("Boss_warrior �R�����a�I");
        }
    }

    public void EnableHitbox()
    {
        if (hitboxCollider != null)
            hitboxCollider.enabled = true;
    }

    public void DisableHitbox()
    {
        if (hitboxCollider != null)
            hitboxCollider.enabled = false;
    }
}
