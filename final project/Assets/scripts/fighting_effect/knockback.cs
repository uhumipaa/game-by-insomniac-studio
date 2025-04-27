using UnityEngine;
using DG.Tweening;

public class Knockback : MonoBehaviour
{
   public float knockbackDistance = 2f; // 擊退距離
    public float knockbackDuration = 0.2f; // 擊退時間
    private Tween currentTween;

    public void ApplyKnockback(Vector2 attackerPos)
    {
        // 計算方向（2D）
        Vector2 direction = (Vector2)transform.position - attackerPos;
        if (direction.magnitude == 0)
            direction = Vector2.right;

        Vector2 knockbackDir = direction.normalized;

        // 強制只在 2D 平面移動，保持 z 軸為 0
        Vector3 currentPos = transform.position;
        Vector3 targetPos = new Vector3(
            currentPos.x + knockbackDir.x * knockbackDistance,
            currentPos.y + knockbackDir.y * knockbackDistance,
            0f // 確保 z 軸不動
        );

        // 若已有擊退動畫在跑，先停止
        if (currentTween != null && currentTween.IsActive())
            currentTween.Kill();

       // 嘗試暫停 enemy_move 的移動
        var moveScript1 = GetComponent<enemy_move>();
        if (moveScript1 != null) moveScript1.canMove = false;
        //playcontrol停止
        var moveScript2 = GetComponent<player_controler>();
        if (moveScript2 != null) moveScript2.canMove = false;

        // 執行 DOTween 位移動畫
        currentTween = transform.DOMove(targetPos, knockbackDuration)
        .SetEase(Ease.OutQuad)
        .OnComplete(() => {
            if (moveScript1 != null) moveScript1.canMove = true;
            if (moveScript2 != null) moveScript2.canMove = true;
        });

        Debug.Log($"[Knockback] 播放擊退動畫，從 {transform.position} 推往 {targetPos}");
    }
}
