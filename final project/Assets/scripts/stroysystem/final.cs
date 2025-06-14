using UnityEngine;
using Flower;
using System.Collections.Generic;
using System.Collections;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;
public class final : MonoBehaviour
{
    FlowerSystem fs;
    public GameObject weapon;
    public Camera camera1;
    public CanvasGroup BlackPanel;
    public Animator[] Gameover;
    public GameObject tobecontinuepanel;
    public PlayableDirector director;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        GameObject.Find("player_battle").transform.position = new Vector3(1f,-4.5f,0);
        GameObject.Find("ToolBar(Clone)").GetComponent<CanvasGroup>().alpha=0;
        if (SaveSystem.instance.jumpstory)
        {

            fs = FlowerManager.Instance.CreateFlowerSystem("default", false);
            fs.SetupDialog();
        }
        else
        {
            fs = FlowerManager.Instance.GetFlowerSystem("default");
        }
        
        fs.RegisterCommand("stopfinal", stopplay);
        fs.RegisterCommand("playfinal", play);
        fs.RegisterCommand("showweapon", showweapon);
        fs.RegisterCommand("die", diedie);
        fs.RegisterCommand("attack", attack);
        fs.RegisterCommand("stopbgm", stopbgm);
            fs.ReadTextFromResource("final");
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            fs.Next();
        }
    }
    void stopplay(List<string> strings)
    {
        director.Pause();
    }
    void play(List<string> strings)
    {
        director.Play();
    }
    void showweapon(List<string> strings)
    {
        weapon.SetActive(true);

    }
    void attack(List<string> strings)
    {
        weapon.GetComponent<Animator>().SetTrigger("attack");
        if(Audio_manager.Instance!=null)
            Audio_manager.Instance.Play(12, "player_sword", false, 0);
    }

    void diedie(List<string> strings)
    {
        BlackPanel.alpha = 1;
        foreach (var gameover in Gameover)
        {
            gameover.SetTrigger("GameOver");
        }
        StartCoroutine(tobecontinue());
    }
    void stopbgm(List<string> strings)
    {
        if (Audio_manager.Instance != null)
        {
            Audio_manager.Instance.Stop();
        }
    }
    IEnumerator tobecontinue()
    {
        yield return new WaitForSecondsRealtime(4f);
        tobecontinuepanel.GetComponent<CanvasGroup>().alpha = 1;
        GameObject.Find("ToolBar(Clone)").GetComponent<CanvasGroup>().alpha = 1;
        tobecontinuepanel.GetComponent<Animator>().SetTrigger("continue");
        yield return new WaitForSecondsRealtime(5f);
        FindAnyObjectByType<Player_Property>().transform.position = Vector2.zero;
        SceneManager.LoadScene("farm");
    }
}
