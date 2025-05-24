using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;
using UnityEngine.Events;

public class DropReceiver : MonoBehaviour
{
    public void Initialize(Inventory_UI inventoryUI)
    {
        EventTrigger trigger = GetComponent<EventTrigger>();
        if (trigger == null)
        {
            trigger = gameObject.AddComponent<EventTrigger>();
        }

        trigger.triggers.Clear();

        EventTrigger.Entry entry = new EventTrigger.Entry
        {
            eventID = EventTriggerType.Drop,
            callback = new EventTrigger.TriggerEvent()
        };

        entry.callback.AddListener((eventData) =>
        {
            if (inventoryUI != null && UI_Manager.draggedSlot != null)
            {
                inventoryUI.StartCoroutine(DelayedRemove(inventoryUI));
            }
            else
            {
                Debug.LogWarning("❗ 無法移除，inventoryUI 或 draggedSlot 為 null");
            }
        });

        trigger.triggers.Add(entry);

        Debug.Log("✅ DropReceiver 已綁定 Inventory_UI.Remove()");
    }

    private IEnumerator DelayedRemove(Inventory_UI inventoryUI)
    {
        float timer = 0f;
        float timeout = 5f;

        while (timer < timeout)
        {
            var gm = FindFirstObjectByType<GameManager>();
            var player = FindFirstObjectByType<player_trigger>();
            var dragged = UI_Manager.draggedSlot;

            if (gm != null && player != null && dragged != null)
            {
                gm.player = player; // 確保補綁
                inventoryUI.Remove();
                yield break;
            }

            timer += Time.deltaTime;
            yield return null;
        }

        Debug.LogError("❌ 5 秒內仍無法執行 Remove()，請檢查 player 是否正確生成於場景中");
    }

}
