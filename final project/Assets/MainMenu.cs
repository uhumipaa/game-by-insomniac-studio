using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    [SerializeField] CanvasGroup loadmenu;
    [SerializeField] Button loadbutton;
    [SerializeField] Button[] loadsavebutton;
    [SerializeField] Animator loadani;
    void Start()
    {
        
    }
    public void NewGame()
    {
        SaveSystem.instance.newgame();
    }

    public void QuitGame()
    {
        Application.Quit();
    }
    
}
