using UnityEngine;

public class InventorySidePanelController : MonoBehaviour
{
    public static InventorySidePanelController Instance;
    public RectTransform inventoryPanel;
    public GameObject sidePanel;
    public float shiftAmount = 150f; // 左移距離
    private bool isOpen = false;
    private Vector3 originalPosition;

    private void Awake()
    {
        Instance = this;
    }
    void Start()
    {
        originalPosition = inventoryPanel.localPosition;
        sidePanel.SetActive(false);
    }

    public void ToggleSidePanel()
    {
        isOpen = !isOpen;

        if (isOpen)
        {
            inventoryPanel.localPosition = originalPosition + new Vector3(-shiftAmount, 0, 0);
            sidePanel.SetActive(true);
        }
        else
        {
            inventoryPanel.localPosition = originalPosition;
            sidePanel.SetActive(false);
        }
    }
}
