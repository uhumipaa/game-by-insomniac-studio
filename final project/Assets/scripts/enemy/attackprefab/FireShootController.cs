using UnityEngine;

public class FireShootController : MonoBehaviour,IEnenmyResetInterface
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public PolygonCollider2D[] hitbox;
    private Animator ani;
    private int hitbox_count = 0;
    private float damage;
    private void Awake()
    {
        ani = GetComponent<Animator>();
    }
    public void Reset()
    {
        Resetcontroller();
        gameObject.SetActive(true);
    }
    public void Resetcontroller()
    {
        hitbox_count = 0;
    }
    public void enablehitbox()
    {
        if (hitbox_count != 0)
        {
            hitbox[hitbox_count].enabled = false;
            hitbox_count++;
            hitbox[hitbox_count].enabled = true;
        }
        else
        {
            hitbox[hitbox_count].enabled = true;
            hitbox_count++;
        }
    }
    public void closehitbox()
    {
        hitbox[hitbox_count].enabled = false;
    }
}
