using UnityEngine;

public class player_controler : MonoBehaviour
{
    private slash slash;
    public GameObject hitbox;
    public Rigidbody2D rb;
    public TileManager tileManager;
    Animator ani;
    private dash Dash;
    private Player_Property property;
    private Vector3 lastFacing = Vector3.one;
<<<<<<< Updated upstream
    public bool canMove = true;

=======
    private float lastattacktime;
    bool attacking;
    private enum direction{ up,down,left,right}
    private direction player_direaction;
>>>>>>> Stashed changes
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        property = GetComponent<Player_Property>();
        
        ani = GetComponent<Animator>();
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
<<<<<<< Updated upstream
        if (!canMove) return; //  防止擊退中還能操作
        if (Input.GetMouseButtonDown(0))
=======
        if (Time.time - lastattacktime > property.attack_time)
>>>>>>> Stashed changes
        {
            attacking = false;
        }
        if (!Dash.dashing)
        {
            if (Input.GetMouseButtonDown(0)&&!attacking)
            {
                
                attack();
            }

            if (Input.GetKeyDown(KeyCode.LeftShift))
            {
                On_dash();
            }
        }
        

        //待查
        /*if(Input.GetMouseButtonDown(1)) //右鍵
        {
            Vector3Int position = new Vector3Int((int)transform.position.x, (int)transform.position.y, 0) ;
            //tileManager = GetComponent<TileManager>();
            if(tileManager.IsInteractable(position))
            {
                Debug.Log("Tile is interactable");
            }
        }*/
    }
        
    void LateUpdate()
    {
        if (!Dash.dashing)
        {
            if (!canMove) return;//擊退時暫停
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
        attacking = true;
        lastattacktime = Time.time;
        Vector3 hitbox_spawn_point = transform.position;
        float Rotationz = 0f;
        switch (player_direaction)
        {
            case direction.up:
                hitbox_spawn_point = transform.position + Vector3.up * property.attackrange;
                Rotationz = 0f;
                break;
            case direction.down:
                hitbox_spawn_point = transform.position + Vector3.down * property.attackrange;
                Rotationz = 180f;
                break;
            case direction.right:
                hitbox_spawn_point = transform.position + Vector3.right * property.attackrange;
                Rotationz = 90f;
                break;
            case direction.left:
                hitbox_spawn_point = transform.position + Vector3.left * property.attackrange;
                Rotationz = 90f;
                break;
        }
        GameObject hit = Instantiate(hitbox, hitbox_spawn_point, Quaternion.Euler(0, 0, Rotationz), transform);
        if (hit != null) { 
        slash = hit.GetComponent<slash>();
        slash.enable_hitbox(property.attack_time);
        } else{
            Debug.Log("abc");
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
            }else if(movehorizontal < 0)
            {
                player_direaction = direction.left;
            }
            ani.SetFloat("vertical", movevetical);
            if (movevetical>0)
            {
                player_direaction = direction.up;
            }else if(movevetical<0){
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
