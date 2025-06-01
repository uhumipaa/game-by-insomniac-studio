using UnityEngine;
public class player_controler : MonoBehaviour
{
    private Rigidbody2D rb;
    public GameObject sword;
    //public TileManager tileManager;
    Animator ani;
    private dash Dash;
    private Player_Property property;
    private Vector3 lastFacing = Vector3.one;
    public bool canMove = true;
    private float lastattacktime;
    bool attacking; 
    private enum direction { up, down, left, right }
    private direction player_direaction;
    public Transform sliderCanvas;
    public bool canControl = true; // ✅ 新增這個

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        property = GetComponent<Player_Property>();
        ani = GetComponent<Animator>();
        sliderCanvas = FindAnyObjectByType<playerhealthbar>()?.transform;
        if (sliderCanvas == null)
        {
            sliderCanvas = transform;
        }
        // 確保 Animator 不會影響 Scale
        if (GetComponent<Animator>() != null)
        {
            GetComponent<Animator>().applyRootMotion = false;
        }
        Dash = GetComponent<dash>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!canControl || !canMove) return; // ✅ 新增 canControl 判斷
        if (Time.time - lastattacktime > property.attack_time)
        {
            attacking = false;
        }
        if (Input.GetMouseButtonDown(0)&&!attacking)
        {
            attack();
        }

        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            On_dash();
        }
    }
        
    void LateUpdate()
    {
        if (!Dash.dashing)
        {
            if (!canControl || !canMove) return; // ✅ 新增 canControl 判斷//擊退時暫停
            Move();

            // ➤ 這裡改為根據移動方向翻轉角色
            if (rb.linearVelocityX > 0)
            {
                transform.localScale = new Vector3(1, 1, 1);
                lastFacing = transform.localScale;
            }// 向右
            else if (rb.linearVelocityX < 0) { 
                transform.localScale = new Vector3(-1, 1, 1); // 向左
                lastFacing = transform.localScale;
            }
        }
        else
        {
            if(rb.linearVelocityX != 0)
            {
                transform.localScale = lastFacing;
            }
        }
    }
    void On_dash()
    {
        Vector2 inputDir = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")).normalized;
        if (inputDir != Vector2.zero)
        {
            Dash.setdireaction(inputDir);
        }
    }
    void attack()
    {
        if (sword == null)
        {
            return;
        }
        attacking = true;
        lastattacktime = Time.time;
        sword.SetActive(true);
        if(Audio_manager.Instance!=null)
            Audio_manager.Instance.Play(12, "player_sword", false, 0);
        switch (player_direaction)
        {
            case direction.up:
                sword.GetComponent<Animator>().SetTrigger("up");
                break;
            case direction.down:
                sword.GetComponent<Animator>().SetTrigger("down");
                break;
            case direction.right:
                sword.GetComponent<Animator>().SetTrigger("right");
                break;
            case direction.left:
                sword.GetComponent<Animator>().SetTrigger("right");
                break;
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
            if (movehorizontal > 0)
            {
                player_direaction = direction.right;
                sliderCanvas.localScale = new Vector3(1, 1f, 1f);//控制血條方向
            }
            else if (movehorizontal < 0)
            {
                player_direaction = direction.left;
                sliderCanvas.localScale = new Vector3(-1, 1f, 1f);//控制血條方向
            }
            ani.SetFloat("vertical", movevetical);
            if (movevetical > 0)
            {
                player_direaction = direction.up;
            }
            else if (movevetical < 0)
            {
                player_direaction = direction.down;
            }
            ani.SetBool("walk", true);
        }
        else
        {
            rb.linearVelocity = Vector2.zero; // 停止移動時歸零速度
            ani.SetBool("walk", false);
        }
    }
}
