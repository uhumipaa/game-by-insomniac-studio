using UnityEngine;

public class PlayerFarmController : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private Rigidbody2D rb;
    private Animator ani;
    private Player_Property property;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        ani = GetComponent<Animator>();
        property = GetComponent<Player_Property>();
    }

    // Update is called once per frame
    void Update()
    {
        Move();
        if (rb.linearVelocityX > 0)
            {
                transform.localScale = new Vector3(1, 1, 1);
            }// 向右
            else if (rb.linearVelocityX < 0) { 
                transform.localScale = new Vector3(-1, 1, 1); // 向左
            }
    }
    private void Move()
    {
        float movehorizontal;
        float movevetical;
        movehorizontal = Input.GetAxis("Horizontal");
        movevetical = Input.GetAxis("Vertical");
        if (movehorizontal != 0||movevetical!=0)
        {
            rb.linearVelocity = new Vector2(movehorizontal * property.speed, movevetical * property.speed);

            ani.SetFloat("horizontal", movehorizontal);
            ani.SetFloat("vertical", movevetical);
            ani.SetBool("walk", true);
        }
        else
        {
            rb.linearVelocity = Vector2.zero; // 停止移動時歸零速度
            ani.SetBool("walk", false);
        }
    }
}
