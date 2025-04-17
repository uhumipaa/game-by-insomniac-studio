using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public TileManager tileManager;

    private void Awake()
    {
        if(instance != null && instance != this){
            Destroy(this.gameObject);
        }
        else{
            instance = this;
        }

        DontDestroyOnLoad(this.gameObject);

        //tlieManager = GetComponent<TlieManager>();

    }

}
