using UnityEngine;
using System.Collections;
using System.Threading;
public class test : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public GameObject firestone;
    public GameObject player;
    private void Start()
    {
        StartCoroutine(cast());
    }
    IEnumerator cast()
    {
        int count=0;
        while (count <= 5)
        {
            Instantiate(firestone).GetComponent<FireStone_controller>().Setdrop(player.transform.position);
            yield return new WaitForSecondsRealtime(3f);
            count++;
        }
        
    }
}
