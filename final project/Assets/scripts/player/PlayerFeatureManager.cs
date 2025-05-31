using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerFeatureManager : MonoBehaviour
{
    [SerializeField] MonoBehaviour[] towerscript;
    [SerializeField] MonoBehaviour[] farmscript;
    //[SerializeField] MonoBehaviour[] homescript;
    void Awake()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;       
    }
    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        string sceneName = scene.name;

        foreach (MonoBehaviour script in towerscript)
        {
            script.enabled = (sceneName == "tower" || sceneName == "battle_test" || sceneName == "town");
        }
        //農場
        foreach (MonoBehaviour script in farmscript)
        {
            script.enabled = (sceneName == "farm" || sceneName == "playerHome");
        }

        /*//在主角家跟NPC家
        foreach (MonoBehaviour script in homescript)
        {
            script.enabled = (sceneName == "playerHome" || sceneName == "VickyHome");
        }*/

        if(sceneName == "farm")
        {
            Audio_manager.Instance.Play(21, "farm_bgm", true, 0);
        }
    
    }
}