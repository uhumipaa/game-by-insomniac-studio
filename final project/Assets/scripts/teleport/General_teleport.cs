using System;
using UnityEngine;
using UnityEngine.Rendering;
public class General_teleport : MonoBehaviour
{
    [SerializeField] CanvasGroup check;
    private TowerManager tower;
    private Maploaders maploader;
    bool isinrange;
    bool opening;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (check == null)
        {
            check = GameObject.Find("towertelecheck").GetComponent<CanvasGroup>();
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

    // Update is called once per frame
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            isinrange = true;
        }
    }
    void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        isinrange = false;
    }
}
