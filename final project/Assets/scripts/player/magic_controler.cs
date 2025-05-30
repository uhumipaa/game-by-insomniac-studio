using UnityEngine;
using System.Collections;
using UnityEngine.U2D.IK;
public class magic_controler : MonoBehaviour
{
    private isMagic magic_current;
    public MonoBehaviour[] magic_list;
    private int magic_now;
    bool iscast;
    public float[] magiccd;
    public bool[] canbecast = { true };
    public Animator staffani; 
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        magic_now = 0;
        switchmagic(0);
    }

    // Update is called once per frame
    void Update()
    {
        if (iscast) return;
        if (Input.GetKeyDown(KeyCode.Space) && canbecast[magic_now])
        {
            StartCoroutine(castwithani());
            StartCoroutine(run_cd(magic_now));
            staffani.SetTrigger("cast");
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
    IEnumerator castwithani()
    {
        iscast = true;
        Audio_manager.Instance.Play(13, "player_swing", false, 0);
        yield return new WaitForSecondsRealtime(1.75f);
        magic_current?.cast();
        iscast = false;
    }
    IEnumerator run_cd(int i)
    {
        canbecast[i] = false;
        yield return new WaitForSecondsRealtime(magiccd[i]);
        canbecast[i] = true;
    }
}
