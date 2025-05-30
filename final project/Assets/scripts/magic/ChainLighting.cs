using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class ChainLighting : MonoBehaviour,isMagic
{
    public int chainnumber;
    public float range;
    public float dmgpara;
    private Player_Property property;
    public float chiandelay;
    public GameObject lightingprefab;
    public Animator ChainLightingCD; // 在 Inspector 連到 CooldownIcon 的 Animator


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        property = GetComponent<Player_Property>();
    }
    public void cast()
    {
        StartCoroutine(Lighting(transform.position, new List<GameObject>(), chainnumber));
        ChainLightingCD.SetTrigger("StartCD");
    }
    private IEnumerator Lighting(Vector3 postion,List<GameObject> hitenemy, int remainingJumps)
    {
        if (remainingJumps <= 0)
        {
            yield break;
        }
        GameObject target = GetComponent<Getclosestenemy>().getclosest(transform.position,hitenemy,range);
        if (target != null)
        {
            createlighting(postion, target.transform.position);
            hitenemy.Add(target);
            target.GetComponent<enemy_property>()?.takedamage(property.magic_atk , Vector2.zero);
        }
        else
        {
            yield break;
        }

            yield return new WaitForSeconds(chiandelay);
        StartCoroutine(Lighting(target.transform.position,hitenemy, remainingJumps-1));

    }
    void createlighting(Vector3 from_pos, Vector3 to_pos)
    {
        GameObject lightning = Instantiate(lightingprefab);

        Vector3 direction = (to_pos - from_pos).normalized;
        float distance = Vector3.Distance(from_pos, to_pos);

        // 放置在兩點之間
        lightning.transform.position = (from_pos + to_pos) / 2;

        // 指向目標
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        lightning.transform.rotation = Quaternion.Euler(0, 0, angle);

        // ✨ 調整縮放：圖片寬度為 35 像素，PPU 為 15，要轉為世界單位寬度為 1
        float baseUnitLength = 35f / 15f;  // 35 像素 ÷ 15 PPU = 2.33 單位（未縮放時的長度）
        float scaleX = distance / baseUnitLength;

        lightning.transform.localScale = new Vector3(scaleX, 1f, 1f); // 等比放大 X 軸

        // 可選：自動摧毀
        Destroy(lightning, 0.2f);
    }

}
