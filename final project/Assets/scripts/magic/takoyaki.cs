using UnityEngine;

public class takoyaki : MonoBehaviour,isMagic
{
    public float speed;
    public GameObject takoyaki_prefab;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
    }

    // Update is called once per frame
    public void cast()
    {
        Vector2 mouse_position = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 direction = (mouse_position - (Vector2)transform.position).normalized;
        Vector2 spown_position = (Vector2)transform.position + direction * 0.5f;
        Instantiate(takoyaki_prefab, spown_position, Quaternion.identity).GetComponent<tako_control>().setdirection(direction);
    }
}
