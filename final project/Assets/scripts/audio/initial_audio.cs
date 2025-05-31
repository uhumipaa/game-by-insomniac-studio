using UnityEngine;

public class initial_audio : MonoBehaviour
{

    public static initial_audio Instance;

    private void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        Debug.Log("222 222");
        Audio_manager.Instance.Play(0, "initial_bgm", true, 6);
        
    }

    public void Transmap()
    {
        Debug.Log("transaudio");
        if (TowerManager.Instance.currentTowerFloor % 10 == 5)
        {
            Debug.Log("111");
            Audio_manager.Instance.Play(21, "farm_bgm", true, 11);
        }else
        {
            Debug.Log("222");
            Audio_manager.Instance.Play(21, "farm_bgm", true, 0);
        }
    }

    // initial_audio.Instance.Transmap();

}
