using UnityEngine;
using Flower;
using System.Collections.Generic;
public class farmscene : MonoBehaviour
{
    FlowerSystem fs;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Audio_manager.Instance.Stop();
        Audio_manager.Instance.Play(21, "farm_bgm", true, 0);
        if (!SaveSystem.instance.firststoryfinish)
        {
            if (SaveSystem.instance.jumpstory)
            {
                fs = FlowerManager.Instance.CreateFlowerSystem("default", false);
                fs.SetupDialog();
            }
            else
            {
                fs = FlowerManager.Instance.GetFlowerSystem("default");
            }
            fs.ReadTextFromResource("farm");
            fs.RegisterCommand("release", release);
            fs.RegisterCommand("lock", playerlock);
            fs.RegisterCommand("bgm", bgm);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            fs.Next();
        }
    }
    void bgm(List<string> strings)
    {
        Audio_manager.Instance.Stop();
        Audio_manager.Instance.Play(21, "farm_bgm", true, 0);
    }
    void release(List<string> strings)
    {
        FindAnyObjectByType<Player_Property>().GetComponent<Player_Property>().speed = 4.5f;
    }
    void playerlock(List<string> strings)
    {
        FindAnyObjectByType<Player_Property>().GetComponent<Player_Property>().speed = 4.5f;
        
    }
}
