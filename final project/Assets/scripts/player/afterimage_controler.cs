using System.Collections;
using Unity.Cinemachine;
using UnityEngine;

public class afterimage_contorler : MonoBehaviour
{
    public float fade_time;
    private Animator ani;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        ani = GetComponent<Animator>();
        StartCoroutine(Fade(fade_time));
    }

    public void Setdirection(float horizontal,float vertical)
    {
        
        ani.SetFloat("horizontal", horizontal);
        ani.SetFloat("vertical", vertical);
        
    }
    IEnumerator Fade(float delay)
    {
        yield return new WaitForSeconds(delay);
        Destroy(gameObject);
    }
    // Update is called once per frame
}
