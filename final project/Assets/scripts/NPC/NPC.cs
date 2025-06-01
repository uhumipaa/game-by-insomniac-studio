using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class DialogueGroup
{
    public string[] lines;
}

public class NPC : MonoBehaviour, interactable
{
    [Header("對話資料")]
    public NPCDialogue dialogueData;

    [Header("UI 參考")]
    public GameObject dialoguePanel;
    public TMP_Text dialogueText, nameText;
    public Image protraitImage;

    [Header("NPC ID")]
    public string npcID = "NPC_Default";

    private int dialogueIndex;
    private bool isTyping, isDialogueActive;
    private string[] currentDialogue;
    // 統計用變數
    public int DialogueCount => FishingStats.DialogueCount;

    void Start()
    {
        PlayerPrefs.DeleteKey("TalkedTo_" + npcID); //清除該 NPC 的對話紀錄
    }
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

        if (isDialogueActive)
        {
            NextLine();
        }
        else
        {
            StartDialogue();
        }
    }

    void StartDialogue()
    {
        //Debug.Log($"HasTalkedBefore = {HasTalkedBefore()}");
        if (!HasTalkedBefore() && dialogueData.firstTimeDialogue.Length > 0)
        {
            //Debug.Log($"首次對話: {npcID}");
            currentDialogue = dialogueData.firstTimeDialogue;
            SetTalked();
            FishingStats.DialogueCount++;

        }
        else
        {
            //Debug.Log($"隨機對話: {npcID}");
            int rand = Random.Range(0, dialogueData.randomDialogues.Count);
            currentDialogue = dialogueData.randomDialogues[rand].lines;
            FishingStats.DialogueCount++ ;
        }

        isDialogueActive = true;
        dialogueIndex = 0;

        nameText.SetText(dialogueData.npcName);
        protraitImage.sprite = dialogueData.npcPortrait;

        dialoguePanel.SetActive(true);
        PauseController.SetPause(true);

        StartCoroutine(TypeLine());
    }

    void NextLine()
    {
        if (isTyping)
        {
            StopAllCoroutines();
            dialogueText.SetText(currentDialogue[dialogueIndex]);
            isTyping = false;
        }
        else if (++dialogueIndex < currentDialogue.Length)
        {
            StartCoroutine(TypeLine());
        }
        else
        {
            EndDialogue();
        }
    }

    IEnumerator TypeLine()
    {
        isTyping = true;
        dialogueText.SetText("");

        foreach (char letter in currentDialogue[dialogueIndex])
        {
            dialogueText.text += letter;
            yield return new WaitForSeconds(dialogueData.typingSpeed);
        }

        isTyping = false;

        if (dialogueData.autoProgressLines.Length > dialogueIndex && dialogueData.autoProgressLines[dialogueIndex])
        {
            yield return new WaitForSeconds(dialogueData.autoProgressDelay);
            NextLine();
        }
    }

    public void EndDialogue()
    {
        StopAllCoroutines();
        isDialogueActive = false;
        dialogueText.SetText("");
        dialoguePanel.SetActive(false);
        PauseController.SetPause(false);
    }

    private bool HasTalkedBefore()
    {
        return PlayerPrefs.GetInt("TalkedTo_" + npcID, 0) == 1;
    }

    private void SetTalked()
    {
        PlayerPrefs.SetInt("TalkedTo_" + npcID, 1);
    }
}
