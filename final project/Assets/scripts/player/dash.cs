using UnityEditor.Experimental.GraphView;
using UnityEngine;
using System.Collections;

public class dash : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public float dash_speed;
    public float dash_cd;
    public float dash_duration;
    private float last_dash_time;
    public bool dashing;
    private Vector2 direction;
    private Rigidbody2D rb;
    private Animator ani;
    private Transform player_pos;
    public GameObject afterimage;
    public void setdireaction(Vector2 Direction)
    {
        
        if ((Time.time - last_dash_time) > dash_cd)
        {
            direction = Direction;
            StartCoroutine(Dash(dash_duration));
        }
    }

    void create_afterImage()
    {
        Instantiate(afterimage, player_pos.position, Quaternion.identity).GetComponent<afterimage_contorler>().Setdirection(ani.GetFloat("horizontal"), ani.GetFloat("vertical"));
    }
    IEnumerator Dash(float delay)
    {
        dashing = true;
        Debug.Log("¶}©ldash");
        rb.linearVelocity = direction * dash_speed;
        float elapsed = 0f;
        float spawnInterval = 0.05f; 
        while (elapsed < delay)
        {
            create_afterImage(); 
            yield return new WaitForSeconds(spawnInterval);
            elapsed += spawnInterval;
        }
        rb.linearVelocity = Vector2.zero;
        dashing = false;
        Debug.Log("Ãö³¬dash");
        last_dash_time = Time.time;
        
    }

    void Start()
    {
        ani = GetComponent<Animator>();
        player_pos = GetComponent<Transform>();
        rb = GetComponent<Rigidbody2D>();
        dashing = false;
    }

    // Update is called once per frame
}
