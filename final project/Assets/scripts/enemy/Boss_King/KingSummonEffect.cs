using UnityEngine;

public class KingSummonEffect : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    public float rotateSpeed = 90f; // 每秒旋轉度數
    public float fadeDuration = 1.5f;

    private float timer = 0f;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        Color c = spriteRenderer.color;
        c.a = 1f;
        spriteRenderer.color = c;
    }

    void Update()
    {
        // 旋轉
        transform.Rotate(Vector3.forward, rotateSpeed * Time.deltaTime);

        // 漸淡
        timer += Time.deltaTime;
        float alpha = Mathf.Lerp(1f, 0f, timer / fadeDuration);
        Color c = spriteRenderer.color;
        c.a = alpha;
        spriteRenderer.color = c;

        if (timer >= fadeDuration)
        {
            Destroy(gameObject);
        }
    }
}
