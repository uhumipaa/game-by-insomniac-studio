using UnityEngine;
using TMPro;
public class shopinfo : MonoBehaviour
{
    public CanvasGroup infopanel;
    public TMP_Text nametext;
    public TMP_Text descriptiontext;
    public TMP_Text[] effecttext;
    private RectTransform infopanelrect;
    private bool isFollowing = false;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        infopanelrect = GetComponent<RectTransform>();
    }
    void Update()
    {
        if (isFollowing)
        {
            followmouse();
        }
    }
    public void showinfo(ItemData item)
    {
        infopanel.alpha = 1;
        nametext.text = item.name;
        //isFollowing = true;
    }
    public void hideinfo()
    {
        infopanel.alpha = 0;
        //isFollowing = false;
    }
    public void followmouse()
    {
        Vector3 mouseposition = Input.mousePosition;
        Vector3 offset = new Vector3(150, -150, 0);
        infopanelrect.position = mouseposition + offset;
    }
}

