using UnityEngine;

public class DinosourBornAnimation : MonoBehaviour
{
    private Animator ani;
    public MonoBehaviour contorller;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        ani = GetComponent<Animator>();
        contorller.enabled = false;
        ani.SetBool("borning", true);
    }

    // Update is called once per frame
    public void FinishBorn()
    {
        Debug.Log("born");
        ani.SetBool("borning",false);
        ani.SetBool("running", true);
        contorller.enabled = true;
    }
}
