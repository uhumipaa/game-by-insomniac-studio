using System;
using UnityEngine;

[CreateAssetMenu(fileName = "NewNPCDialogue", menuName = "NPC Dialogue")]
public class NPCDialogue : ScriptableObject
{
    public String npcName;
    public Sprite npcPortrait;
    public string[] dialogueLines;
    public bool[] autoProgressLines; //自動觸發的台詞
    public float autoProgressDelay = 1.5f;
    public float typingSpeed = 0.05f;
    //public AudioClip VoiceSound;
    public float voicePitch = 1f;
    
}
