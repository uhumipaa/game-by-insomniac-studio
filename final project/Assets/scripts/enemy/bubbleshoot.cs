using UnityEngine;

public class BubbleShooter : MonoBehaviour
{
    public GameObject bubblePrefab;  
    public float shootInterval = 3f; // 發射間隔
    private float timer;
    private Transform player;

    void Start()
    {
        timer = shootInterval;
        player = GameObject.FindGameObjectWithTag("Player")?.transform;

        if (player == null)
        {
            Debug.LogError("找不到Player");
        }
    }
    
    void Update()
    {
        if (player == null) return;

        timer -= Time.deltaTime; //計時

        if (timer <= 0f)
        {
            ShootBubble();
            timer = shootInterval;
        }
    }

    void ShootBubble()
    {
        GameObject bubble = Instantiate(bubblePrefab, transform.position, Quaternion.identity); //生成泡泡
        bubbletracker tracker = bubble.GetComponent<bubbletracker>();

        if (tracker != null)
        {
            tracker.SetTarget(player);
        }
    }
}
