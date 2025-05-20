using UnityEngine;

public class face_to_player : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public Transform player; 
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
       // 5. 面向玩家
        Vector3 scale = transform.localScale;
        float originalX = Mathf.Abs(scale.x); // 保留原本的大小
        scale.x = player.position.x < transform.position.x ? -originalX : originalX;
        transform.localScale = scale; 
    }
}
