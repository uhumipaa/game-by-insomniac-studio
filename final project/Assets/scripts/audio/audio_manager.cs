using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Audio_manager : MonoBehaviour
{
    List<AudioSource> audios = new List<AudioSource>();
    void Start()
    {
        for(int i = 0; i < 9; i++) // ���X�ӴN i < �X��
        {
            var audio = this.gameObject.AddComponent<AudioSource>();
            audios.Add(audio);
        }
        
    }

    void Update()
    {
        
    }
}
