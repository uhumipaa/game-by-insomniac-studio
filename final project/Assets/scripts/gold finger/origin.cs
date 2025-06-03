using UnityEngine;

public class origin : MonoBehaviour
{
    private Transform player;

    public void origintransform()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;

        // 將位置設為 (0, 0, 0)
        player.transform.position = new Vector3(0f, 0f, 0f);
    }
}