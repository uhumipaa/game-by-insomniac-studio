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
    private static readonly string key = "meowmeow"; // 加密密碼
    private bool useencryption=false;

    public SaveFileHandler(string DataDirPath, string DataFileName, bool useencryption)
    {
        this.DataDirPath = DataDirPath;
        this.DataFileName = DataFileName;
        this.useencryption = useencryption;
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
                using (FileStream stream = new FileStream(fullpath, FileMode.Open))
                {
                    using (StreamReader reader = new StreamReader(stream))
                    {
                        datatoload = reader.ReadToEnd();
                    }
                }
                if (useencryption)
                {
                    datatoload = EncryptDecrypt(datatoload);
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
            if (useencryption)
            {
                datastore = EncryptDecrypt(datastore);
            }
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
    private string EncryptDecrypt(string data)
    {
        char[] result = new char[data.Length];
        for (int i = 0; i < data.Length; i++)
        {
            result[i] = (char)(data[i] ^ key[i % key.Length]);
        }
        return new string(result);
    }
    //測試有沒有存檔
    public bool HasSaveFile()
    {
        string fullpath = Path.Combine(DataDirPath, DataFileName);
        return File.Exists(fullpath);
    }
}
