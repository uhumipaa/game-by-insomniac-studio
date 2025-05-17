using UnityEngine;
using static Hero;


[CreateAssetMenu(fileName = "ArmorRewardData", menuName = "Scriptable Objects/ArmorRewardData")]
public class ArmorRewardData : RewardData
{
    public string armorID;
    public int add_def;
    public int add_atk;
    public int add_HP;
    public float diff_cd;
    public int add_magic;
    public override void ApplyReward(GameObject player)
    {
        playerstatus staus= player.GetComponent<playerstatus>();
        staus.add_status(this);
    }
}
