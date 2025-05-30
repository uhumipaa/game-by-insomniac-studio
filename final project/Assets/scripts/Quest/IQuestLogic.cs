using UnityEngine;

public interface IQuestLogic
{
    bool IsComplete();        // 是否完成任務
    void GiveReward();        // 發送獎勵
    
}
