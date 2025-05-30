using Unity.Cinemachine;
using UnityEngine;

public class CarmeraAutoFindTarget : MonoBehaviour
{

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [System.Obsolete]
    void Start()
    {
        GameObject player = FindAnyObjectByType<Player_Property>().gameObject;
        CinemachineCamera vcam = GetComponent<CinemachineCamera>();
    if (vcam != null && player != null)
    {
            vcam.Follow = player.transform;
    }
    }

    // Update is called once per frame
}
