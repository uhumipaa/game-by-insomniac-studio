using UnityEngine;

public class Crystal : MonoBehaviour
{
    [Header("Boss 參考 (由 Inspector 指定)")]
    public DarkMagicionController boss;

    private bool isDestroyed = false;

    private void OnDisable()
    {
        if (!isDestroyed && boss != null)
        {
            OnCrystalDestroyed();
        }
    }

    //  不處理傷害， 只通知 Boss
    private void OnCrystalDestroyed()
    {
        isDestroyed = true;

        boss.TakeDamageFromCrystal(); // 呼叫 Boss 的扣血函式
        Debug.Log($"{gameObject.name} 被破壞，通知 Boss 扣血");
    }
    }


