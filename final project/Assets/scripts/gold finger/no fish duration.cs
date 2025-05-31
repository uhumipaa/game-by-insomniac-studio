using UnityEngine;

public class nofishduration : MonoBehaviour
{
    public WaitingFishAnimationCustom fish_ani;
    public FishingUIManager fish_delay;
    public void no_fish_duration()
    {
        if (fish_ani == null)
        {
            fish_ani = GameObject.Find("WaitingFishText")?.GetComponent<WaitingFishAnimationCustom>();
        }
        if (fish_delay == null)
        {
            fish_delay = GameObject.Find("FishingUIManagerObject")?.GetComponent<FishingUIManager>();
        }
        if (fish_ani != null && fish_delay != null)
        {
            fish_ani.totalDuration = 0;
            fish_delay.duration = 0;
        }
    }
}
