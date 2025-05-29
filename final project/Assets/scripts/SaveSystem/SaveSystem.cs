using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System.Collections;
using System.Text.RegularExpressions;
[System.Serializable]
public class SaveData
{
    public PlayerStatusData playerStatusData;
    public SaveData()
    {
        playerStatusData = new PlayerStatusData();
    }
}
public class SaveSystem : MonoBehaviour
{
    [Header("儲存資料的東東")]
    [SerializeField] private string filename;


    SaveData savedata;
    List<ISaveData> saveDatasObject;
    private SaveFileHandler saveFileHandler;
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
    }
    void Start()
    {
        this.saveFileHandler = new SaveFileHandler(Application.persistentDataPath, filename);
        this.saveDatasObject = findallsavedata();
    }
    public void Savegame()
    {
        foreach (ISaveData saveData in saveDatasObject) {
            saveData.LoadData(savedata);
        }
    }
    public void loadgame()
    {
        this.savedata = saveFileHandler.Load();
        foreach (ISaveData saveData in saveDatasObject)
        {
            saveData.LoadData(savedata);
        }
    }
    public void newgame()
    {

    }
    void OnApplicationQuit()
    {
        Savegame();
    }
    private List<ISaveData> findallsavedata() {
        IEnumerable<ISaveData> saveDatas = Object.FindObjectsByType<MonoBehaviour>(FindObjectsSortMode.None)
                .OfType<ISaveData>();
        return new List<ISaveData>(saveDatas);
    }
}
 