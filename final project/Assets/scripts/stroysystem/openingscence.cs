using UnityEngine;
using Flower;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine.Playables;
public class openingscence : MonoBehaviour
{
    FlowerSystem fs;
    public GameObject[] enemys;
    public PlayableDirector director;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        GameObject.Find("ToolBar(Clone)").GetComponent<CanvasGroup>().alpha=0;
        fs = FlowerManager.Instance.GetFlowerSystem("default");
        fs.RegisterCommand("stopdirect", stopplay);
        fs.RegisterCommand("playdirect", play);
        fs.ReadTextFromResource("opening");
        director.Stop();
        fs.RegisterCommand("next", autonext);
        fs.RegisterCommand("summon", summon);
        fs.RegisterCommand("setuptoturiol", settoplaytutorial);
    }
    void autonext(List<string> strings)
    {
        fs.Next();
    }
    void stopplay(List<string> strings)
    {
        director.Pause();
    }
    void play(List<string> strings)
    {
        director.Play();
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            fs.Next();
        }
    }
    void summon(List<string> strings)
    {
        foreach (GameObject enemy in enemys)
        {
            enemy.SetActive(true);
        }
    }
    private void settoplaytutorial(List<string> ya)
    {
        GameObject player = GameObject.Find("player_battle");
        GameObject.Find("ToolBar(Clone)").GetComponent<CanvasGroup>().alpha=0;
        player.GetComponent<SpriteRenderer>().enabled=true;
        GameObject.Find("escmenu").GetComponent<CanvasGroup>().alpha=0;
        player.transform.position = Vector2.zero;
    }
}
