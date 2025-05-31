using UnityEngine;
using Flower;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
public class IntroSenceController : MonoBehaviour
{
    FlowerSystem fs;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        settoplaystory();
        //�]�w�ø��Jintro
        fs = FlowerManager.Instance.CreateFlowerSystem("default", false);
        fs.SetupDialog();
        fs.ReadTextFromResource("intro");
        fs.RegisterCommand("load_scene", (List<string> load_scene) =>
        {
            SceneManager.LoadScene(load_scene[0]);
        });
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            fs.Next();
        }
    }
    private void settoplaystory()
    {
        GameObject.Find("player_battle").GetComponent<SpriteRenderer>().enabled=false;
    }
    
    
    
}
