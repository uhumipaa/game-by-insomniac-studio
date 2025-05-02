using UnityEngine;

[CreateAssetMenu(fileName = "ExtraReward", menuName = "Scriptable Objects/ExtraReward")]
public class ExtraRewardData : ScriptableObject
{
    public string extra_name;
    public int amount;
    public void ApplyExtra(GameObject player)
    {
        Debug.Log($"��o�B�~���y�G{extra_name} x{amount}");
        // ��ڵ����D��A�Ҧp�GplayerInventory.Add(extraName, amount);
    }
}
