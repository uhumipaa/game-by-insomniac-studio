using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Audio_manager : MonoBehaviour
{
    public AudioClip initial_bgm; //0
    public AudioClip map1map10_bgm;
    public AudioClip map11map20_bgm;
    public AudioClip map21map30_bgm;
    public AudioClip map31map40_bgm;
    public AudioClip map41map50_bgm; //5
    public AudioClip Boss_king_bgm;
    public AudioClip Boss_Dark_Magicion_bgm;
    public AudioClip Boss_warrior_bgm;
    public AudioClip Boss_dino_bgm;
    public AudioClip finalboss_bgm; //10
    public AudioClip restmap_bgm;
    public AudioClip player_sword;
    public AudioClip player_swing;
    public AudioClip player_take_damaged;
    public AudioClip player_tako; //15
    public AudioClip player_lighting;
    public AudioClip Boss_king_sword;
    public AudioClip Boss_king_summon;
    public AudioClip Boss_king_clean;
    public AudioClip Boss_king_teleport; //20
    public AudioClip farm_bgm;

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

        for (int i = 0; i < 22; i++) // 看上面public有幾個
        {
            Debug.Log($"Audio Count: {audios.Count}");

            var audio = this.gameObject.AddComponent<AudioSource>();
            audios.Add(audio);
        }
    }
    public void Stop()
    {
        foreach(AudioSource audio in audios)
        {
            if (audio.isPlaying)
            {
                audio.Stop();
            }
        }
    }

    public void Play(int index, string name, bool isLoop,int lastindex)
    {
        var clip = GetAudioClip(name);
        if (clip == null)
        {
            Debug.LogWarning("音效不存在：" + name);
            return;
        }
        var lastaudio = audios[lastindex];
        var audio = audios[index];
        if (lastaudio.isPlaying)
        {
            Debug.Log("Stopaudio");
            lastaudio.Stop(); // 確保停止
        }
        // 若已經正在播放該 Clip，則略過重複播放
        /*
        if (audio.clip == clip)
        {
            Debug.Log("正在播放相同音樂，略過播放");
            return;
        }
        audio.Stop();
        // 若播放的是不同的音樂，要先 Stop 原本的
        if (audio.clip != clip)
        {
            Debug.Log("stop");
            audio.Stop();
            Debug.Log("stop2");
        }
        */
        Debug.Log($"audio.isplaying: {audio.isPlaying} | audio.clip: {audio.clip} | audio.index{index}| lastaudio: {lastaudio.clip}");
        Debug.Log("22222");
        audio.clip = clip;
        audio.loop = isLoop;
        audio.Play();
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

            case "player_sword":
                return player_sword;
            case "player_swing":
                return player_swing;
            case "player_take_damaged":
                return player_take_damaged;

            case "player_tako":
                return player_tako;
            case "player_lighting":
                return player_lighting;

            case "Boss_king_sword":
                return Boss_king_sword;
            case "Boss_king_summon":
                return Boss_king_summon;
            case "Boss_king_clean":
                return Boss_king_clean;
            case "Boss_king_teleport":
                return Boss_king_teleport;

            case "farm_bgm":
                return farm_bgm;

            default:
                return  initial_bgm;
        }
    }
}
