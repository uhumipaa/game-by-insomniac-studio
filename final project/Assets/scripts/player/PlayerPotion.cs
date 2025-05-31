using System.Collections;
using UnityEngine;

public class PlayerPotion : MonoBehaviour
{
    private Player_Property player_Property;
    private Transform player;
    private bool isUsingPotion = false; // ✅ 防止連續使用
    public GameObject atkeffect;
    public GameObject defeffect;
    public GameObject hpeffect;
    public GameObject acceffect;

    void Start()
    {
        player_Property = FindAnyObjectByType<Player_Property>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftAlt))
        {
            usepotion();
        }
    }

    void usepotion()
    {
        if (isUsingPotion) // ✅ 若正在使用藥水，不可再用
        {
            Debug.Log("⚠️ 正在作用藥水效果中，無法再次使用。");
            return;
        }

        var selectedSlot = InventoryManager.Instance.toolbar.selectedSlot;
        if (selectedSlot == null || selectedSlot.itemData == null || selectedSlot.count <= 0 || selectedSlot.IsEmpty)
            return;

        var item = selectedSlot.itemData;

        // ✅ 根據類型執行不同效果
        if (item.type == ItemType.Atkpotion)
        {
            var renderer = GetComponentInChildren<Renderer>();
            if (renderer != null)
            {
                // ✅ 記住原始顏色
                Color originalColor = renderer.material.color;

                // ✅ 改變顏色為紫色
                renderer.material.color = Color.magenta;

                // ✅ 開始協程：過一段時間後再恢復
                StartCoroutine(RevertColorAfterDuration(renderer, originalColor, item.duration));
            }
            Instantiate(atkeffect, transform.position, Quaternion.identity);
            StartCoroutine(potioneffect(item.duration, item));
            var allSlots = FindObjectsOfType<Slot_UI>();
            foreach (var slot in allSlots)
            {
                slot.Refresh();
            }
        }
        if (item.type == ItemType.Defpotion)
        {
            var renderer = GetComponentInChildren<Renderer>();
            if (renderer != null)
            {
                // ✅ 記住原始顏色
                Color originalColor = renderer.material.color;

                // ✅ 改變顏色為藍色
                renderer.material.color = new Color(0.5f, 0.7f, 1f);

                // ✅ 開始協程：過一段時間後再恢復
                StartCoroutine(RevertColorAfterDuration(renderer, originalColor, item.duration));
            }
            Instantiate(defeffect, transform.position, Quaternion.identity);
            StartCoroutine(potioneffect(item.duration, item));
            var allSlots = FindObjectsOfType<Slot_UI>();
            foreach (var slot in allSlots)
            {
                slot.Refresh();
            }
        }
        if (item.type == ItemType.Accpotion)
        {
            var renderer = GetComponentInChildren<Renderer>();
            if (renderer != null)
            {
                // ✅ 記住原始顏色
                Color originalColor = renderer.material.color;

                // ✅ 改變顏色為綠色
                renderer.material.color = Color.green;

                // ✅ 開始協程：過一段時間後再恢復
                StartCoroutine(RevertColorAfterDuration(renderer, originalColor, item.duration));
            }
            Instantiate(acceffect, transform.position, Quaternion.identity);
            StartCoroutine(potioneffect(item.duration, item));
            var allSlots = FindObjectsOfType<Slot_UI>();
            foreach (var slot in allSlots)
            {
                slot.Refresh();
            }
        }
        if (item.type == ItemType.Hppotion)
        {
            Instantiate(hpeffect, transform.position, Quaternion.identity);
            player_Property.current_health += 10;
            player_Property.update_property();
            InventoryManager.Instance.TryRemove("Toolbar", item, 1);
            Debug.Log($"✅ 使用 {item.itemName}，立即提升 HP。");
            var allSlots = FindObjectsOfType<Slot_UI>();
            foreach (var slot in allSlots)
            {
                slot.Refresh();
            }
        }
        selectedSlot.count--;
    }

    IEnumerator potioneffect(float duration, ItemData item)
    {
        isUsingPotion = true; // ✅ 標記正在作用中

        PlayerStatusManager.instance.add_status(item);
        InventoryManager.Instance.TryRemove("Toolbar", item, 1);
        Debug.Log($"✅ 使用藥水：{item.itemName}，效果持續 {duration} 秒");

        yield return new WaitForSecondsRealtime(duration);

        PlayerStatusManager.instance.diff_status(item);
        Debug.Log($"⏱️ 藥水效果結束：{item.itemName}");

        isUsingPotion = false; // ✅ 解鎖使用
    }

    IEnumerator RevertColorAfterDuration(Renderer renderer, Color originalColor, float duration)
    {
        yield return new WaitForSecondsRealtime(duration);
        if (renderer != null)
        {
            renderer.material.color = originalColor;
        }
    }
}