using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerFeatureManager : MonoBehaviour
{
    [SerializeField] MonoBehaviour[] towerscript;
    [SerializeField] MonoBehaviour[] farmscript;
    void Awake()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;       
    }
    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        string sceneName = scene.name;

        foreach (MonoBehaviour script in towerscript)
        {
            script.enabled = (sceneName == "tower"||sceneName == "battle_test");
        }
        //農場
        foreach (MonoBehaviour script in farmscript)
        {
            script.enabled = (sceneName == "farm");
        }
    
    }
}