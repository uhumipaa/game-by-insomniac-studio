using UnityEngine;

public class GolemAttackEffect : MonoBehaviour
{
    public GameObject attackEffectPrefab;
    public Transform effectSpawnPoint;
    public float effectLifetime = 0.05f; // 🕒 特效存活時間（秒）

    public void PlayAttackEffect()
    {
        // 產生特效
        GameObject effect = Instantiate(attackEffectPrefab, effectSpawnPoint.position, Quaternion.identity);

        // 自動銷毀特效
        Destroy(effect, effectLifetime);
    }
}
