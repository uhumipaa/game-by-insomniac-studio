using UnityEngine;

public class RandomMover2D : MonoBehaviour
{
    public float moveSpeed = 2f;//速度
    public float directionChangeInterval = 2f;///何時改變方向
    public float invisibilityInterval = 5f;//隱形間隔
    public float invisibilityDuration = 1f;//隱形持續時間

    public Rect moveBounds = new Rect(-10, -5, 20, 10);//界線

    private Vector2 moveDirection;//目前移動方向
    private float directionTimer;//何時換方向
    private float invisibilityTimer;//何時隱形
    private float invisibilityTimeRemaining;//隱形剩餘時間
    private bool isInvisible = false;

    private SpriteRenderer spriteRenderer;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        ChooseNewDirection();
        directionTimer = directionChangeInterval;
        invisibilityTimer = invisibilityInterval;
    }

    void Update()
    {
        // 移動
        transform.Translate(moveDirection * moveSpeed * Time.deltaTime);

        // 碰到邊界就反彈
        Vector3 pos = transform.position;
        Vector2 halfSize = GetHalfSize();

        // 左或右邊界
        if (pos.x - halfSize.x < moveBounds.xMin || pos.x + halfSize.x > moveBounds.xMax)
        {
            moveDirection.x = -moveDirection.x;
            pos.x = Mathf.Clamp(pos.x, moveBounds.xMin + halfSize.x, moveBounds.xMax - halfSize.x);
            transform.position = pos;
        }

        // 上或下邊界
        if (pos.y - halfSize.y < moveBounds.yMin || pos.y + halfSize.y > moveBounds.yMax)
        {
            moveDirection.y = -moveDirection.y;
            pos.y = Mathf.Clamp(pos.y, moveBounds.yMin + halfSize.y, moveBounds.yMax - halfSize.y);
            transform.position = pos;
        }

        // 換方向
        directionTimer -= Time.deltaTime;
        if (directionTimer <= 0f)
        {
            ChooseNewDirection();
            directionTimer = directionChangeInterval;
        }

        // 隱形
        invisibilityTimer -= Time.deltaTime;

        if (!isInvisible && invisibilityTimer <= 0f)
        {
            SetVisibility(false);
            isInvisible = true;
            invisibilityTimeRemaining = invisibilityDuration;
        }

        if (isInvisible)
        {
            invisibilityTimeRemaining -= Time.deltaTime;
            if (invisibilityTimeRemaining <= 0f)
            {
                SetVisibility(true);
                isInvisible = false;
                invisibilityTimer = invisibilityInterval;
            }
        }
    }

    //移動方向選擇
    void ChooseNewDirection()
    {
        Vector2[] directions = new Vector2[]
        {
            Vector2.up,
            Vector2.down,
            Vector2.left,
            Vector2.right,
            (Vector2.up + Vector2.left).normalized,
            (Vector2.up + Vector2.right).normalized,
            (Vector2.down + Vector2.left).normalized,
            (Vector2.down + Vector2.right).normalized
        };

        int index = Random.Range(0, directions.Length);
        moveDirection = directions[index];
    }

    //隱形或顯示
    void SetVisibility(bool visible)
    {
        if (spriteRenderer != null)
        {
            spriteRenderer.enabled = visible;
        }
    }

    //界線計算
    Vector2 GetHalfSize()
    {
        if (spriteRenderer == null)
            return Vector2.zero;

        Bounds bounds = spriteRenderer.bounds;
        return bounds.extents;
    }
}