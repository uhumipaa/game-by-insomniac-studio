using UnityEngine;

[CreateAssetMenu(fileName = "RewardData", menuName = "Scriptable Objects/RewardData")]
public abstract class RewardData : ScriptableObject
{
    public enum Rarity
    {
        rare,superrare,epic,langendary
    };
    public Sprite icon;
    public string rewardname;
    public string statement;
    public Rarity rarity;
    public abstract void ApplyReward(GameObject player);
}
