using UnityEngine;
using UnityEngine.EventSystems;
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

        entry.callback.AddListener((eventData) => inventoryUI.Remove());
        trigger.triggers.Add(entry);

        Debug.Log("✅ DropReceiver 已綁定 Inventory_UI.Remove()");
    }
}
