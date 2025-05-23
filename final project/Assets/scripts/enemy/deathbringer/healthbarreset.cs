using UnityEngine;

public class healthbarreset : MonoBehaviour
{
    private EnemySummoner otransform;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        otransform = GetComponentInParent<EnemySummoner>();
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = otransform.originalPosition;
    }
}
