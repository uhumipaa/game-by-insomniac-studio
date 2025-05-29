using System;
using System.IO;
using System.Runtime.InteropServices;
using UnityEditor.Playables;
using UnityEngine;

public class SaveFileHandler : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private string DataDirPath = "";
    private string DataFileName = "";

    public SaveFileHandler(string DataDirPath, string DataFileName)
    {
        this.DataDirPath = DataDirPath;
        this.DataFileName = DataFileName;
    }
    public SaveData Load()
    {
        string fullpath = Path.Combine(DataDirPath, DataFileName);
        SaveData loaddata = null;
        if (File.Exists(fullpath))
        {
            try
            {
                //讀取加密的資料
                string datatoload = "";
                using (FileStream stream = new FileStream(fullpath, FileMode.Create))
                {
                    using (StreamReader reader = new StreamReader(stream))
                    {
                        datatoload = reader.ReadToEnd();
                    }
                }
                //解密資料
                loaddata = JsonUtility.FromJson<SaveData>(datatoload);
            }
            catch (Exception e)
            {
                Debug.LogError("Error to load data to file:" + fullpath + "\n" + e);
            }
        }
        
         

            return loaddata;
    }
    public void Save(SaveData data)
    {
        string fullpath = Path.Combine(DataDirPath, DataFileName);
        try
        {
            Directory.CreateDirectory(Path.GetDirectoryName(fullpath));
            string datastore = JsonUtility.ToJson(data, true);
            using (FileStream stream = new FileStream(fullpath, FileMode.Create))
            {
                using (StreamWriter writer = new StreamWriter(stream))
                {
                    writer.Write(datastore);
                }
            }
        }
        catch (Exception e)
        {
            Debug.LogError("Error to save data to file:" + fullpath + "\n" + e);
        }
        
    }

}
