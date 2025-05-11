using UnityEngine;

public class knightcontroiller : MonoBehaviour
{

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    // void Start()
    // {
        
    // }

    // Update is called once per frame
    void Update()
    {
        Vector3 currentEuler = transform.eulerAngles;
        currentEuler.z = 0f;
        transform.eulerAngles = currentEuler;
    }
}
