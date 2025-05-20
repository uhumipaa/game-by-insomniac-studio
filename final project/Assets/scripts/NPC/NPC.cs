using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class NPC : MonoBehaviour, interactable
{
    public NPCDialogue dialogueData;
    public GameObject dialoguePanel;
    public TMP_Text dialogueText, nameText;
    public Image protraitImage;

    private int dialogueIndex;
    private bool isTyping, isDialogueActive;

    public bool CanInteract()
    {
        return !isDialogueActive;
    }

    public void Interact()
    {
        if (dialogueData == null)
        {
            return;
        }

        if (isDialogueActive) //如果對話正在進行中
        {
            //NextLine
        }
        else
        {
            //StartDialogue
        }
    }

    void StartDialogue()
    {
        //狀態變為正在對話
        isDialogueActive = true;

        //從第一條對話開始
        dialogueIndex = 0;

        //設置NPC資料
        nameText.SetText(dialogueData.npcName);
        protraitImage.sprite = dialogueData.npcPortrait;

        //打開對話框
        dialoguePanel.SetActive(true);

    }
}
