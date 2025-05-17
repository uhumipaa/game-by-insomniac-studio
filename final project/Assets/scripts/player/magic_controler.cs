using UnityEngine;

public class magic_controler : MonoBehaviour
{
    private isMagic magic_current;
    public MonoBehaviour[] magic_list;
    private int magic_now;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        magic_now = 0;
        switchmagic(0);
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            magic_current?.cast();
        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            Debug.Log("change");
            magic_now = (magic_now + 1) % magic_list.Length;
            switchmagic(magic_now);
        }
    }
    void switchmagic(int i)
    {
        magic_current = magic_list[i] as isMagic;
        
    }
}
