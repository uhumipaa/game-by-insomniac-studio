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
        loadmenu.alpha = 0;
        loadmenu.interactable = false;
        loadmenu.blocksRaycasts = false;
        if (!SaveSystem.instance.HasSaveFile(0))
        {
            loadbutton.interactable = false;
        }
        else
        {
            loadbutton.interactable = true;
        }
        for (int i = 0; i < loadsavebutton.Length; i++)
        {
            if (!SaveSystem.instance.HasSaveFile(i))
            {
                loadsavebutton[i].interactable = false;
            }
            else
            {
                loadsavebutton[i].interactable = true;
            }
        }
    }
    public void NewGame()
    {
        SaveSystem.instance.newgame();
    }

    public void QuitGame()
    {
        Application.Quit();
    }
    public void Openloadmenu()
    {
        loadmenu.alpha = 1;
        loadmenu.interactable = true;
        loadmenu.blocksRaycasts = true;
        loadani.SetTrigger("open");
    }
    public void closeloadmenu()
    {
        loadmenu.alpha = 0;
        loadmenu.interactable = false;
        loadmenu.blocksRaycasts = false;
    }
    
}
