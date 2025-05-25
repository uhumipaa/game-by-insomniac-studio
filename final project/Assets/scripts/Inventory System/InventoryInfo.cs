using UnityEngine;
using TMPro;
using UnityEngine.UI;
public class InventoryInfo : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public CanvasGroup infopanel;
    public TMP_Text nametext;
    public TMP_Text descriptiontext;
    public Image icon;
    private RectTransform infopanelrect;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        infopanelrect = GetComponent<RectTransform>();
    }
    public void showinfo(ItemData item)
    {
        infopanel.alpha = 1;
        nametext.text = item.name;
        descriptiontext.text = item.description;
        icon.sprite = item.icon;
    }
    public void hideinfo()
    {
        infopanel.alpha = 0;
        nametext.text = "";
        descriptiontext.text = "";
    }
    public void followmouse()
    {
        Vector3 mouseposition = Input.mousePosition;
        Vector3 offset = new Vector3(80,30, 0);
        infopanelrect.position = mouseposition + offset;
    }
}

