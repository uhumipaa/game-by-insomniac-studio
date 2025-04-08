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
        if(Input.GetKeyDown(KeyCode.Space))
        {
            
        }
    }
    void LateUpdate()
    {
        // ➤ 這裡改為根據移動方向翻轉角色
        if (rb.linearVelocityX > 0)
            transform.localScale = new Vector3(1, 1, 1);  // 向右
        else if (rb.linearVelocityX < 0)
            transform.localScale = new Vector3(-1, 1, 1); // 向左
    }

    private void OnTriggerStay2D(Collider2D coll)
    {
        
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
            ani.SetFloat("horizontal", Mathf.Abs(movehorizontal));
            ani.SetBool("walk", true);
        }
        else
        {
            rb.linearVelocity = Vector2.zero; // 停止移動時歸零速度
            ani.SetBool("walk", false);
        }
    }
}
