using UnityEngine;

public class Firestone : MonoBehaviour
{
    private FireStone_controller controller;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        controller = GetComponentInParent<FireStone_controller>();
    }

    // Update is called once per frame
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Hint"))
        {
            Debug.Log("4s5as");
            controller.Meet();
        }
    }
}
