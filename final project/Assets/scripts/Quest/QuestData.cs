using UnityEngine;

[CreateAssetMenu(fileName = "NewQuest", menuName = "Quest System/Quest Data")]
public class QuestData : ScriptableObject
{
    public Sprite rewardIcon;
    public string goalText;
    public string rewardDescription;

    public ScriptableObject questLogic;

    [HideInInspector] public bool isCompleted = false;
    [HideInInspector] public bool rewardClaimed = false;
}
