using System.Collections;
using UnityEngine;

public class NanoMachine_Son : MonoBehaviour
{
    private bool isInvincible = false;
    private Coroutine invincibleCoroutine;

    [Header("NanoMachine Settings")]
    [SerializeField] private float invincibleDuration = 0.5f; // 無敵時間（秒）

    // 呼叫這個方法來啟動無敵
    public void StartInvincibility()
    {
        if (invincibleCoroutine != null)
        {
            StopCoroutine(invincibleCoroutine);
        }
        invincibleCoroutine = StartCoroutine(InvincibleRoutine());
    }

    // 讀取目前是不是無敵中
    public bool IsInvincible()
    {
        return isInvincible;
    }

    // 控制無敵時間
    private IEnumerator InvincibleRoutine()
    {
        Debug.Log("進入無敵狀態 (NanoMachine)");
        isInvincible = true;

        yield return new WaitForSeconds(invincibleDuration);

        isInvincible = false;
        Debug.Log("結束無敵狀態 (NanoMachine)");
    }
}
