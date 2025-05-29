using UnityEngine;

public interface ISaveData
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public void SaveData(ref SaveData saveData);
    public void LoadData(SaveData saveData);
}
