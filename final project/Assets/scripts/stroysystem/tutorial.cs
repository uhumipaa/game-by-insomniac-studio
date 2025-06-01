using UnityEngine;
using Flower;
using System.Collections.Generic;
public class tutorial : MonoBehaviour
{
    public enemy_property[] enemys;
    bool finish;
    FlowerSystem fs;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        asettoplaytutorial();
        fs = FlowerManager.Instance.GetFlowerSystem("default");
        fs.ReadTextFromResource("tutorialstart");
        fs.RegisterCommand("timestart", timestart);
        fs.RegisterCommand("timestop", timestop);
        fs.RegisterCommand("setfarm", farmsetting);
    }


    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            fs.Next();
        }
        if (FindAnyObjectByType<enemy_property>() == null&&!finish)
        {
            finish = true;
            fs.ReadTextFromResource("tutorialfinish");
        }
    }
    // Update is called once per frame
    void timestart(List<string> strings)
    {
        FindAnyObjectByType<Player_Property>().GetComponent<Player_Property>().speed = 4.5f;
        foreach (enemy_property enemy in enemys)
        {
            enemy.speed = 3;
        }
    }

    void timestop(List<string> strings)
    {
        FindAnyObjectByType<Player_Property>().GetComponent<Player_Property>().speed = 0;
        foreach (enemy_property enemy in enemys)
        {
            enemy.speed = 0;
        }
    }
    void farmsetting(List<string> strings)
    {
        GameObject player = GameObject.Find("player_battle");
        player.transform.position = Vector2.zero;
    }
    private void asettoplaytutorial()
    {
        GameObject player = GameObject.Find("player_battle");
        GameObject.Find("ToolBar(Clone)").GetComponent<CanvasGroup>().alpha=1;
        player.GetComponent<SpriteRenderer>().enabled=true;
        GameObject.Find("escmenu").GetComponent<CanvasGroup>().alpha=1;
        player.transform.position = Vector2.zero;
    }
}
