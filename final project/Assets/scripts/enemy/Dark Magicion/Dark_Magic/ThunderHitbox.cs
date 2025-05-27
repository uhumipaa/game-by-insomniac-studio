using UnityEngine;

public class ThunderHitbox : MonoBehaviour
{
    public GameObject hitbox;  // 指定一個 Hitbox 物件（例如 BoxCollider 連 hitbox controller）

    // 打開 hitbox → 一般由動畫事件呼叫
    public void EnableHitbox()
    {
        if (hitbox != null)
        {
            var controller = hitbox.GetComponent<Hitbox_Controller>();
            if (controller != null)
            {
                controller.Enablecol();
            }
            else
            {
                Debug.LogWarning("Hitbox_Controller 未掛載在指定的 hitbox 上！");
            }
        }
    }

    // 關閉 hitbox → 通常動畫事件末尾呼叫
    public void DisableHitbox()
    {
        if (hitbox != null)
        {
            var controller = hitbox.GetComponent<Hitbox_Controller>();
            if (controller != null)
            {
                controller.Closecol();
            }
            else
            {
                Debug.LogWarning("Hitbox_Controller 未掛載在指定的 hitbox 上！");
            }
        }
    }

    // ✅ 關閉整個 Prefab 本身（例如在動畫結束後由事件觸發）
    public void ClosePrefab()
    {
        gameObject.SetActive(false);
    }
}
