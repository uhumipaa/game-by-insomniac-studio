using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewNPCDialogue", menuName = "NPC Dialogue")]
public class NPCDialogue : ScriptableObject
{
    public String npcName;
    public Sprite npcPortrait;
    [Header("首次對話（只出現一次）")]
    public string[] firstTimeDialogue;

    [Header("隨機對話（從中隨機選一組）")]
    public List<DialogueGroup> randomDialogues;

    public bool[] autoProgressLines; //自動觸發的台詞
    public float autoProgressDelay = 1.5f;
    public float typingSpeed = 0.05f;
    //public AudioClip VoiceSound;
    //public float voicePitch = 1f;

    [System.Serializable]
    public class DialogueGroup
    {
        public string[] lines;
    }

}
