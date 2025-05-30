using UnityEngine;

public class Defbutton : MonoBehaviour
{
    public void OnButtonClick()
    {
        // 在場景中尋找第一個帶有 Player_property 的物件
        Player_Property player = FindFirstObjectByType<Player_Property>();

        if (player != null)
        {
            player.DefAdd(); // 呼叫方法
        }
        else
        {
            Debug.LogWarning("找不到 Player_property！");
        }
    }
}
