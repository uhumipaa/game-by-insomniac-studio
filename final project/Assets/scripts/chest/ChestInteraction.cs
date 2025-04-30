using UnityEngine;
using UnityEngine.UI;
public class ChestInteraction : MonoBehaviour
{
    public GameObject interactionUI;
    private bool isPlayerIn = false;
    private bool isOpened = false;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Debug.Log("aaa");
        interactionUI.SetActive(false);
    }
    private void Update()
    {
        if (isPlayerIn && !isOpened)
        {
            if (Input.GetKeyDown(KeyCode.Space)){
                open();
                isOpened = true;
            }
        }
    }
    // Update is called once per frame
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Debug.Log("aaa");
            isPlayerIn = true;
            interactionUI.SetActive(true);
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            interactionUI.SetActive(false);
            isPlayerIn = false;
        }
    }
    void open()
    {
        interactionUI.SetActive(false);
        Debug.Log("open!");
    }
}
