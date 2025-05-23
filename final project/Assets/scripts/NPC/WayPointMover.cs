using System.Collections;
using UnityEngine;

public class WayPointMover : MonoBehaviour
{
    public Transform waypointParent;
    public float moveSpeed = 2f;
    public float waitTime = 2f;
    public bool loopWaypoints = true;

    private Transform[] waypoints;
    private int currentWaypointIndex;
    private bool isWaiting;

    private Animator animator;
    private Vector2 lastPosition;

    //方向冷卻時間
    private Vector2 lastMoveDir = Vector2.down; // 初始方向
    private float directionChangeCooldown = 0.3f; // 幾秒內不換方向
    private float lastDirectionChangeTime = 0f;

    private SpriteRenderer spriteRenderer;

    void Start()
    {
        //初始化waypoints
        waypoints = new Transform[waypointParent.childCount];
        for (int i = 0; i < waypointParent.childCount; i++)
        {
            waypoints[i] = waypointParent.GetChild(i);
        }

        // 初始化動畫器與位置
        animator = GetComponent<Animator>();
        lastPosition = transform.position;

        //初始化圖片
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        //如果暫停或等待中，不移動
        if (PauseController.IsGamePaused || isWaiting)
        {
            animator.SetFloat("Speed", 0); // 靜止動畫
            return;
        }

        MoveToWayPoint();
    }

    void MoveToWayPoint()
    {
        Transform target = waypoints[currentWaypointIndex];
        Vector2 currentPosition = transform.position;
        Vector2 targetPosition = target.position;
        Vector2 moveDelta = (targetPosition - currentPosition).normalized;

        transform.position = Vector2.MoveTowards(currentPosition, targetPosition, moveSpeed * Time.deltaTime);

        float speed = ((Vector2)transform.position - lastPosition).magnitude / Time.deltaTime;
        UpdateAnimation(moveDelta, speed);

        lastPosition = transform.position;

        if (Vector2.Distance(transform.position, targetPosition) < 0.1f)
        {
            StartCoroutine(WaitAtWayPoint());
        }
    }

    IEnumerator WaitAtWayPoint()
    {
        isWaiting = true;
        animator.SetFloat("Speed", 0); // 停下時播放 Idle
        yield return new WaitForSeconds(waitTime);

        currentWaypointIndex = loopWaypoints ? (currentWaypointIndex + 1) % waypoints.Length : Mathf.Min(currentWaypointIndex + 1, waypoints.Length - 1);
        isWaiting = false;
    }

    void UpdateAnimation(Vector2 moveDir, float speed)
    {
        animator.SetFloat("Speed", speed);
        float now = Time.time;

        //決定主方向
        Vector2 primaryDir;
        if (Mathf.Abs(moveDir.y) > Mathf.Abs(moveDir.x) * 1.4f) //當y變量明顯多於x變量，才會播放上下動畫
        {
            // 垂直方向一有動作就切上/下
            primaryDir = new Vector2(0, moveDir.y > 0 ? 1 : -1);
        }
        else
        {
            // 否則判定為水平移動
            primaryDir = new Vector2(moveDir.x > 0 ? 1 : -1, 0);
        }

        //如果正在移動且方向改變->改變動畫參數
        if (speed > 0.01f)
        {
            if (primaryDir != lastMoveDir && now - lastDirectionChangeTime > directionChangeCooldown)
            {
                lastMoveDir = primaryDir;
                lastDirectionChangeTime = now;

                animator.SetFloat("Horizontal", lastMoveDir.x);
                animator.SetFloat("Vertical", lastMoveDir.y);
            }
        }
        else
        {
            animator.SetFloat("Horizontal", lastMoveDir.x);
            animator.SetFloat("Vertical", lastMoveDir.y);
        }

        //方向向左 -> 根據方向設定filp
        if (lastMoveDir.x != 0)
        {
            spriteRenderer.flipX = lastMoveDir.x < 0;
        }
    }
}
