using UnityEngine;

public class player_controler : MonoBehaviour
{
    public Rigidbody2D rb;
    Animator ani;
    public float speed;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        ani = GetComponent<Animator>();
        // 確保 Animator 不會影響 Scale
        if (GetComponent<Animator>() != null)
        {
            GetComponent<Animator>().applyRootMotion = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        Move();
    }
    void LateUpdate()
    {
        // 強制固定角色大小
        transform.localScale = new Vector3(1, 1, 1);
    }
    private void Move()
    {
        float movehorizontal;
        float movevetical;
        movehorizontal = Input.GetAxis("Horizontal");
        movevetical = Input.GetAxis("Vertical");
        if (movehorizontal != 0||movevetical!=0)
        {
            rb.linearVelocity = new Vector2(movehorizontal * speed, movevetical * speed);
            ani.SetFloat("vertical", movevetical);
            ani.SetFloat("horizontal", movehorizontal);
            ani.SetBool("walk", true);
        }
        else
        {
            rb.linearVelocity = Vector2.zero; // 停止移動時歸零速度
            ani.SetBool("walk", false);
        }
    }
}
