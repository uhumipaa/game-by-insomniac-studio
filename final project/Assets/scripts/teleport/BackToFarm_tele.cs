using UnityEngine;
public class BackToFarm_tele : MonoBehaviour
{
    [SerializeField] CanvasGroup check;
    bool isinrange;
    bool opening;
    void Awake()
    {
        if (check == null)
        {
            check = GameObject.Find("backtofarmcheck").GetComponent<CanvasGroup>();
        }
        check.alpha = 0;
        check.blocksRaycasts = false;
        check.interactable = false;
    }
    void Update()
    {
        if (isinrange && Input.GetKeyDown(KeyCode.Return) && !opening)
        {
            check.alpha = 1;
            check.blocksRaycasts = true;
            check.interactable = true;
            opening = true;
        }
        else if (opening && Input.GetKeyDown(KeyCode.Return))
        {
            check.alpha = 0;
            check.blocksRaycasts = false;
            check.interactable = false;
            opening = false;
        }
    }
    void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        isinrange = true;
    }
    void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        isinrange = false;
    }
}
