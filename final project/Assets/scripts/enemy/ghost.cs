using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ghost : MonoBehaviour
{
    public float moveSpeed = 2f; // 移動速度
    public float directionChangeInterval = 2f; // 何時改變方向
    public float invisibilityInterval = 5f; // 隱形間隔
    public float invisibilityDuration = 1f; // 隱形持續時間
    public Rect moveBounds = new Rect(-10, -5, 20, 10); // 活動範圍界線

    private Vector2 moveDirection; // 目前移動方向
    private float directionTimer; // 換方向間隔
    private float invisibilityTimer; // 隱形間隔
    private float invisibilityTimeRemaining; // 隱形剩餘時間
    private bool isInvisible = false; // 是否正在隱形中

    private SpriteRenderer spriteRenderer;
    private Slider HpSlider; // 血條

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        HpSlider = GetComponentInChildren<Slider>(true); // 找到子物件中的Slider
        ChooseNewDirection(); // 初始隨機方向
        directionTimer = directionChangeInterval;
        invisibilityTimer = invisibilityInterval;
    }

    void Update()
    {
        //移動
        transform.Translate(moveDirection * moveSpeed * Time.deltaTime);

        //翻轉角色面向移動方向
        if (moveDirection.x != 0)
        {
            Vector3 scale = transform.localScale;
            scale.x = moveDirection.x > 0 ? Mathf.Abs(scale.x) : -Mathf.Abs(scale.x);
            transform.localScale = scale;
        }

        //碰到邊界就反彈
        Vector3 pos = transform.position;
        Vector2 halfSize = GetHalfSize();

        //左或右邊界
        if (pos.x - halfSize.x < moveBounds.xMin || pos.x + halfSize.x > moveBounds.xMax)
        {
            moveDirection.x = -moveDirection.x;
            pos.x = Mathf.Clamp(pos.x, moveBounds.xMin + halfSize.x, moveBounds.xMax - halfSize.x);
            transform.position = pos;
        }

        //上或下邊界
        if (pos.y - halfSize.y < moveBounds.yMin || pos.y + halfSize.y > moveBounds.yMax)
        {
            moveDirection.y = -moveDirection.y;
            pos.y = Mathf.Clamp(pos.y, moveBounds.yMin + halfSize.y, moveBounds.yMax - halfSize.y);
            transform.position = pos;
        }

        //換方向
        directionTimer -= Time.deltaTime;
        if (directionTimer <= 0f)
        {
            ChooseNewDirection();
            directionTimer = directionChangeInterval;
        }

        //隱形邏輯
        invisibilityTimer -= Time.deltaTime;

        if (!isInvisible && invisibilityTimer <= 0f)
        {
            SetVisibility(false); // 淡出隱形
            isInvisible = true;
            invisibilityTimeRemaining = invisibilityDuration;
        }

        if (isInvisible)
        {
            invisibilityTimeRemaining -= Time.deltaTime;
            if (invisibilityTimeRemaining <= 0f)
            {
                SetVisibility(true); // 淡入恢復可見
                isInvisible = false;
                invisibilityTimer = invisibilityInterval;
            }
        }
    }

    //移動方向選擇
    void ChooseNewDirection()
    {
        float angle = Random.Range(0f, 360f);
        float rad = angle * Mathf.Deg2Rad;
        moveDirection = new Vector2(Mathf.Cos(rad), Mathf.Sin(rad)).normalized;
    }

    //隱形或顯示
    void SetVisibility(bool visible)
    {
        StopAllCoroutines(); // 停止前一次透明動畫
        StartCoroutine(FadeVisibility(visible));
    }

    //透明度漸變
    IEnumerator FadeVisibility(bool visible)
    {
        float duration = 0.5f; // 淡入淡出時間
        float startAlpha = spriteRenderer.color.a;
        float endAlpha = visible ? 1f : 0f;
        float elapsed = 0f;

        if (visible)
            HpSlider.gameObject.SetActive(true); // 顯示血條

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / duration);
            float alpha = Mathf.Lerp(startAlpha, endAlpha, t);
            Color color = spriteRenderer.color;
            color.a = alpha;
            spriteRenderer.color = color;
            yield return null;
        }

        //確保完全透明或顯示
        Color finalColor = spriteRenderer.color;
        finalColor.a = endAlpha;
        spriteRenderer.color = finalColor;

        if (!visible)
            HpSlider.gameObject.SetActive(false); // 隱藏血條
    }

    //取得 Sprite 尺寸（半徑）
    Vector2 GetHalfSize()
    {
        if (spriteRenderer == null)
            return Vector2.zero;

        Bounds bounds = spriteRenderer.bounds;
        return bounds.extents;
    }
}