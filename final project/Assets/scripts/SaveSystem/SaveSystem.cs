using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System.Collections;
using System.Text.RegularExpressions;
[System.Serializable]
public class SaveData
{
    public PlayerStatusData playerStatusData;
    public Vector3 playerposition;
    public List<SaveItem> backpackItems = new List<SaveItem>();
    public List<SaveItem> storeboxItems = new List<SaveItem>();
    public List<SaveItem> toolbarItems = new List<SaveItem>();
    public List<SaveEquippment> equippmentItems = new List<SaveEquippment>();
    public SaveData()
    {
        playerStatusData = new PlayerStatusData();
    }
}
public class SaveSystem : MonoBehaviour
{
    [Header("儲存資料的東東")]
    [SerializeField] private string basefilename = "save";

    SaveData savedata;
    List<ISaveData> saveDatasObject;
    private SaveFileHandler saveFileHandler;
    [SerializeField] private bool useencryption;
    private int currentSlot = 1;
    public static SaveSystem instance { get; private set; }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        if (instance != null)
        {
            Debug.LogError("has other savesystem");
            Destroy(instance);
        }
        instance = this;
        DontDestroyOnLoad(gameObject);
    }
    void Start()
    {
        SetSaveSlot(0);
        this.saveDatasObject = findallsavedata();
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
        foreach (ISaveData saveData in saveDatasObject)
        {
            saveData.LoadData(savedata);
        }
    }
    public void newgame()
    {
        savedata = new SaveData();
    }
    void OnApplicationQuit()
    {
        Savegame(0);
    }
    //自動綁定所有需要儲存的程式
    private List<ISaveData> findallsavedata()
    {
        IEnumerable<ISaveData> saveDatas = Object.FindObjectsByType<MonoBehaviour>(FindObjectsSortMode.None)
                .OfType<ISaveData>();
        return new List<ISaveData>(saveDatas);
    }
    //可以設定是第幾個存檔
    public void SetSaveSlot(int slot)
    {
        this.currentSlot = slot;
        string Filename = basefilename + "_" + slot + ".json";
        this.saveFileHandler = new SaveFileHandler(Application.persistentDataPath, Filename, useencryption);
    }
    //測試有沒有存檔
    public bool HasSaveFile()
    {
        return saveFileHandler != null && saveFileHandler.HasSaveFile();
    }
}

 