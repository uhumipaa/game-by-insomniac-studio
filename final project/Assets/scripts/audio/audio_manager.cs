using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Audio_manager : MonoBehaviour
{
    public AudioClip initial_bgm;
    public AudioClip map1map10_bgm;
    public AudioClip map11map20_bgm;
    public AudioClip map21map30_bgm;
    public AudioClip map31map40_bgm;
    public AudioClip map41map50_bgm;
    public AudioClip Boss_king_bgm;
    public AudioClip Boss_Dark_Magicion_bgm;
    public AudioClip Boss_warrior_bgm;
    public AudioClip Boss_dino_bgm;
    public AudioClip finalboss_bgm;
    public AudioClip restmap_bgm;

    public static Audio_manager Instance;


    List<AudioSource> audios = new List<AudioSource>();

    private void Awake()
    {
        Debug.Log("AudioManager Awake called");
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);

        for (int i = 0; i < 12; i++) // 看上面public有幾個
        {
            Debug.Log($"Audio Count: {audios.Count}");

            var audio = this.gameObject.AddComponent<AudioSource>();
            audios.Add(audio);
        }
    }

    public void Play(int index, string name, bool isLoop)
    {
        Debug.Log("77777");
        var clip = GetAudioClip(name);
        //Debug.Log($"Clip Name: {clip?.name} | Index: {index}");
        if (clip != null)
        {
            Debug.Log("sjdfi");
            var audio = audios[index];
            audio.clip = clip;
            audio.loop = isLoop;
            audio.Play();
        }
    }

    AudioClip GetAudioClip (string name)
    {
        Debug.Log("55555");
        switch (name)
        {
            case "initial_bgm":
                return initial_bgm;
            case "map1map10_bgm":
                return map1map10_bgm;
            case "map11map20_bgm":
                return map11map20_bgm;
            case "map21map30_bgm":
                return map21map30_bgm;
            case "map31map40_bgm":
                return map31map40_bgm;
            case "map41map50_bgm":
                return map41map50_bgm;
            case "Boss_king_bgm":
                return Boss_king_bgm;
            case "Boss_Dark_Magicion_bgm":
                return Boss_Dark_Magicion_bgm;
            case "Boss_warrior_bgm":
                return Boss_warrior_bgm;
            case "Boss_dino_bgm":
                return Boss_dino_bgm;
            case "finalboss_bgm":
                return finalboss_bgm;
            case "restmap_bgm":
                return restmap_bgm;
            default:
                return  initial_bgm;
        }
    }
}
