using UnityEngine;
using UnityEngine.SceneManagement;
public class SummonHealth : MonoBehaviour, isMagic
{
    public GameObject healthAreaPrefab;
    public Animator HealthAreaCD; // 在 Inspector 連到 CooldownIcon 的 Animator

    public void CastHealArea(Vector2 targetPosition)
    {
        if (healthAreaPrefab == null)
        {
            Debug.LogWarning("尚未設定 Healing prefab！");
            return;
        }
        Debug.LogWarning("summon succes");
        GameObject instance = Instantiate(healthAreaPrefab, targetPosition, Quaternion.identity);
        instance.SetActive(true); // 保險啟用
    }
    void OnEnable()
    {
        Debug.Log("trytofindcd");
        if(SceneManager.GetActiveScene().name=="tower")
            HealthAreaCD = GameObject.Find("HealthAreaCD").GetComponent<Animator>();        
    }

    public void cast()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            CastHealArea(player.transform.position);
        }
        if(HealthAreaCD!=null)
            HealthAreaCD.SetTrigger("StartCD");
    }
}
