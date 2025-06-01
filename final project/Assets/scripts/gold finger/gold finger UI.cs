using UnityEngine;

public class goldfingerUI : MonoBehaviour
{
    public GameObject inventoryPanel;

    private bool isInventoryVisible = false;
    void Start()
    {
        inventoryPanel.SetActive(isInventoryVisible);
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            InventoryManager.Instance.ClearBackpackAndToolbar();
        }

        if (Input.GetKeyDown(KeyCode.G))
        {
            isInventoryVisible = true;
            inventoryPanel.SetActive(isInventoryVisible);
        }

    }
}
