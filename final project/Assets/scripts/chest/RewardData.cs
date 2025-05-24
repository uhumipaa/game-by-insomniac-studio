using UnityEngine;

[CreateAssetMenu(fileName = "RewardData", menuName = "Scriptable Objects/RewardData")]
public abstract class RewardData : ScriptableObject
{
 
    public Sprite icon;
    public string rewardname;
    public string statement;

    public Sprite type;
    public abstract void ApplyReward(GameObject player);
}
