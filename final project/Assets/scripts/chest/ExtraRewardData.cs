using UnityEngine;

[CreateAssetMenu(fileName = "ExtraReward", menuName = "Scriptable Objects/ExtraReward")]
public class ExtraRewardData : ScriptableObject
{
    public string extra_name;
    public int amount;
    public void ApplyExtra(GameObject player)
    {
        Debug.Log($"獲得額外獎勵：{extra_name} x{amount}");
        // 實際給予道具，例如：playerInventory.Add(extraName, amount);
    }
}
