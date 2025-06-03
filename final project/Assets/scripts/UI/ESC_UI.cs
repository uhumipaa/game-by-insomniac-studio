using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class ESC_UI : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [SerializeField] CanvasGroup escpanel;
    [SerializeField] CanvasGroup loadmenu;
    [SerializeField] Button loadbutton;
    [SerializeField] Button loadbuttoninesc;
    [SerializeField] Animator loadani;
    [SerializeField] Animator saveani;
    [SerializeField] CanvasGroup savemenu;
    [SerializeField] Button[] loadsavebutton;
    bool isopen;
    static public ESC_UI instance;
    void Start()
    {
        if (instance != null)
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(this.gameObject);
        escpanel.alpha = 0;
        escpanel.interactable = false;
        escpanel.blocksRaycasts = false;
        loadmenu.alpha = 0;
        loadmenu.interactable = false;
        loadmenu.blocksRaycasts = false;
        savemenu.alpha = 0;
        savemenu.interactable = false;
        savemenu.blocksRaycasts = false;
        if (!SaveSystem.instance.HasSaveFile(0))
        {
            loadbutton.interactable = false;
        }
        else
        {
            loadbutton.interactable = true;
        }
    }
    void Update()
    {
        int sceneIndex = SceneManager.GetActiveScene().buildIndex;
        if (!isopen && Input.GetKeyDown(KeyCode.Escape) && sceneIndex<=4&&sceneIndex>=2)
        {
            openmenu();
        }
        else if (isopen && Input.GetKeyDown(KeyCode.Escape))
        {
            closemenu();
        }
    }
    public void openmenu()
    {
        isopen = true;
        escpanel.alpha = 1;
        escpanel.interactable = true;
        escpanel.blocksRaycasts = true;
        loadmenu.alpha = 0;
        loadmenu.interactable = false;
        loadmenu.blocksRaycasts = false;
        savemenu.alpha = 0;
        savemenu.interactable = false;
        savemenu.blocksRaycasts = false;
        if (!SaveSystem.instance.HasSaveFile(0))
        {
            loadbuttoninesc.interactable = false;
        }
        else
        {
            loadbuttoninesc.interactable = true;
        }
    }
    public void closemenu()
    {
        isopen = false;
        escpanel.alpha = 0;
        escpanel.interactable = false;
        escpanel.blocksRaycasts = false;
        closesavemenu();
        closeloadmenu();
    }
    public void opensavemenu()
    {
        saveani.SetTrigger("open");
        savemenu.alpha = 1;
        savemenu.interactable = true;
        savemenu.blocksRaycasts = true;
        closeloadmenu();
    }
    public void Openloadmenu()
    {
        loadani.SetTrigger("open");
        loadmenu.alpha = 1;
        loadmenu.interactable = true;
        loadmenu.blocksRaycasts = true;
        closesavemenu();
        for (int i = 0; i < loadsavebutton.Length; i++)
        {
            if (!SaveSystem.instance.HasSaveFile(i))
            {
                loadbuttoninesc.interactable = true;
                loadsavebutton[i].interactable = false;
            }
            else
            {
                loadsavebutton[i].interactable = true;
            }
        }
    }
    public void closeloadmenu()
    {
        loadmenu.alpha = 0;
        loadmenu.interactable = false;
        loadmenu.blocksRaycasts = false;
    }
    public void closesavemenu()
    {
        savemenu.alpha = 0;
        savemenu.interactable = false;
        savemenu.blocksRaycasts = false;
    }
    public void backtomainmenu()
    {
        closemenu();
        SaveSystem.instance.Savegame(0);
        SceneManager.LoadScene("Main Menu");
    }
}
