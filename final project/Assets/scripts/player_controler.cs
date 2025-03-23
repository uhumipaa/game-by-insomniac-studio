using UnityEngine;

public class player_controler : MonoBehaviour
{
    public Rigidbody2D rb;
    public float speed;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Move();
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
        }
        else
        {
            rb.linearVelocity = Vector2.zero; // 停止移動時歸零速度
        }
    }
}
