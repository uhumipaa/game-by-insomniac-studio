using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerFeatureManager : MonoBehaviour
{
    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        string sceneName = scene.name;

        GetComponent<player_controler>().enabled = (sceneName == "tower");
        GetComponent<PlayerFarmController>().enabled = (sceneName == "farm");
    }
}