using UnityEngine;
using TMPro;
public class IventoryStatus : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public TMP_Text[] status;
    public void setstatus()
    {
        status[0].text = new("HP:" + PlayerStatusManager.instance.playerStatusData.maxHP);
        status[1].text = new("ATK:" + PlayerStatusManager.instance.playerStatusData.attack);
        status[2].text = new("MP" + PlayerStatusManager.instance.playerStatusData.magic_power);
        status[3].text = new("DEF:" + PlayerStatusManager.instance.playerStatusData.defense);
        status[4].text = new("Spped:" + PlayerStatusManager.instance.playerStatusData.speed);
        status[5].text = new("Luck:" + PlayerStatusManager.instance.playerStatusData.Luck);
    }
}
