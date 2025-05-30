using Unity.Mathematics;
using UnityEngine;
using UnityEngine.SceneManagement;
public class teleporthint : MonoBehaviour
{
    [SerializeField] CanvasGroup canvasGroup;
    [SerializeField] Maploaders maploader;
    public int judge;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public void yes()
    {
        canvasGroup.alpha = 0;
        canvasGroup.interactable = false;
        canvasGroup.blocksRaycasts = false;
        SceneManager.LoadScene("farm");
    }
    public void no()
    {
        canvasGroup.alpha = 0;
        canvasGroup.interactable = false;
        canvasGroup.blocksRaycasts = false;
    }

    public void nextfloar()
    {
        canvasGroup.alpha = 0;
        canvasGroup.interactable = false;
        canvasGroup.blocksRaycasts = false;
        //防止在休息室一直刷通關數
        if (TowerManager.Instance.backtotower)
        {
            TowerManager.Instance.backtotower = false;
        }
        Debug.Log("floor:" + TowerManager.Instance.currentTowerFloor);
        TowerManager.Instance.currentTowerFloor++;
        TowerManager.Instance.finishfloorthistime++;
        Debug.Log("floor:" + TowerManager.Instance.currentTowerFloor);
        if (TowerManager.Instance.currentTowerFloor % 10 == 0)//boss
        {
            judge = TowerManager.Instance.currentTowerFloor / 10;
            maploader.loadBossmap(judge);
        }
        else if (TowerManager.Instance.currentTowerFloor % 10 == 5)//rest
        {
            maploader.loadrestmap();
        }
        else
        {
            int roomVariant = UnityEngine.Random.Range(0, 4);
            maploader.LoadMaps(roomVariant);
        }
        maploader.playbgm();
    }
}
