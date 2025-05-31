using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System.Collections;
using System.Text.RegularExpressions;
using UnityEngine.SceneManagement;
[System.Serializable]
public class SaveData
{
    public PlayerStatusData playerStatusData;
    public Vector3 playerposition;
    public List<SaveItem> backpackItems = new List<SaveItem>();
    public List<SaveItem> storeboxItems = new List<SaveItem>();
    public List<SaveItem> toolbarItems = new List<SaveItem>();
    public int currentfloor;
    public int currentprefab;
    public List<SaveEquippment> equippmentItems = new List<SaveEquippment>();
    public string currentscene;
    public float coin;
    public SaveData()
    {
        playerStatusData = new PlayerStatusData();
        currentscene = "farm";
        playerposition = Vector2.zero;
    }
}
public class SaveSystem : MonoBehaviour
{
    [Header("儲存資料的東東")]
    [SerializeField] private string basefilename = "save";

    SaveData savedata;
    [SerializeField] List<ISaveData> saveDatasObject;
    private SaveFileHandler saveFileHandler;
    [SerializeField] private bool useencryption;
    private int currentSlot = 1;
    public static SaveSystem instance { get; private set; }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        if (instance != null)
        {
            Debug.Log("has other savesystem");
            Destroy(gameObject);
        }
        instance = this;
        DontDestroyOnLoad(gameObject);
        SetSaveSlot(0);
        this.saveDatasObject = findallsavedata();
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.L)) Savegame(1);
    }
    public void Savegame(int slot)
    {
        SetSaveSlot(slot);
        foreach (ISaveData saveData in saveDatasObject)
        {
            saveData.SaveData(ref savedata);
        }
        saveFileHandler.Save(savedata);
    }
    public void loadgame(int slot)
    {
        SetSaveSlot(slot);
        this.savedata = saveFileHandler.Load();
        SceneManager.LoadScene(savedata.currentscene);
        foreach (ISaveData saveData in saveDatasObject)
        {
            saveData.LoadData(savedata);
        }
    }
    public void newgame()
    {
        savedata = new SaveData();
        Audio_manager.Instance.Stop();
        SceneManager.LoadScene("intro");
    }
    void OnApplicationQuit()
    {
        if (SceneManager.GetActiveScene().name != "Main Menu")
        {
            Savegame(0);
        }
    }
    //自動綁定所有需要儲存的程式
    private List<ISaveData> findallsavedata()
    {
        IEnumerable<ISaveData> saveDatas =  FindAllInterfaceImplementations<ISaveData>();
        //IEnumerable<ISaveData> saveDatas = Object.FindObjectsByType<MonoBehaviour>(FindObjectsInactive.Include, FindObjectsSortMode.None)
        ///.OfType<ISaveData>();
        return new List<ISaveData>(saveDatas);
    }
    //可以設定是第幾個存檔
    public void SetSaveSlot(int slot)
    {
        this.currentSlot = slot;
        string Filename = basefilename + "_" + slot + ".json";
        this.saveFileHandler = new SaveFileHandler(Application.persistentDataPath, Filename, useencryption);
        if (saveFileHandler == null) Debug.Log("jfias");
        Debug.Log($"[SetSaveSlot] slot: {slot}, file:{Filename}");
    }
    //測試有沒有存檔
    public bool HasSaveFile(int slot)
    {
        SetSaveSlot(slot);

        return saveFileHandler != null && saveFileHandler.HasSaveFile();
    }
    public static List<T> FindAllInterfaceImplementations<T>() where T : class
    {
    List<T> result = new List<T>();

    var allMono = Resources.FindObjectsOfTypeAll<MonoBehaviour>();

    foreach (var mono in allMono)
    {
        if (mono is T t)
        {
            result.Add(t);
        }
    }

    return result;
}
}

 