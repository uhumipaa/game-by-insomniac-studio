using System.Net.NetworkInformation;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class throw_thing_controller : MonoBehaviour
{
    private Vector2 Start;
    private Vector2 End;
    private Vector2 now;
    public AnimationCurve heightCurve;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        Start = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        float total_distance = Vector2.Distance(Start, End);
        float now_distance = Vector2.Distance(Start, now);
        float t = now_distance / total_distance;
        if (t >= 1)
        {
            explosion();
        }
        Vector2 pos = Vector2.Lerp(Start, End, t);
        float height = heightCurve.Evaluate(t);
        pos.y += height;
        transform.position = pos;
        float scale = 1f - height * 0.5f;
        transform.localScale = new Vector3(scale, scale, 1f);
    }
    void explosion()
    {
        Destroy(gameObject);
    }
    public void Set_parabola(Vector2 end)
    {
        End = end;

    }
}
