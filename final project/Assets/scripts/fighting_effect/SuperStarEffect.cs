using System.Collections;
using UnityEngine;

public class SuperStarEffect : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    private bool isInvincible = false;
    private Coroutine invincibleCoroutine;

    [Header("Superstar Effect Settings")]
    [SerializeField] private float invincibleDuration = 1.0f; // 無敵總時間
    [SerializeField] private float flashInterval = 0.1f;       // 閃爍一次的時間

    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer == null)
        {
            Debug.LogError("SuperstarEffect 找不到 SpriteRenderer！");
        }
    }

    public void StartSuperstar()
    {
        if (invincibleCoroutine != null)
        {
            StopCoroutine(invincibleCoroutine);
        }
        invincibleCoroutine = StartCoroutine(InvincibleRoutine());
    }

    public bool IsInvincible()
    {
        return isInvincible;
    }

    private IEnumerator InvincibleRoutine()
    {
        isInvincible = true;

        int flashCount = Mathf.RoundToInt(invincibleDuration / (flashInterval * 2)); 
        // 例如：0.5 秒 ÷ (0.1s 開 + 0.1s 關) = 2.5 次 → 四捨五入 3 次閃爍

        for (int i = 0; i < flashCount; i++)
        {
            if (spriteRenderer != null)
            {
                spriteRenderer.enabled = false; // 關掉
            }
            yield return new WaitForSeconds(flashInterval);

            if (spriteRenderer != null)
            {
                spriteRenderer.enabled = true; // 打開
            }
            yield return new WaitForSeconds(flashInterval);
        }

        if (spriteRenderer != null)
        {
            spriteRenderer.enabled = true; // 保證最後結束時是亮的
        }
        isInvincible = false;
    }
}
