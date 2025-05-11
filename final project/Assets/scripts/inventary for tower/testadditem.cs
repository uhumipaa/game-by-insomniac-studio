using UnityEngine;

    public class TestItemAdder : MonoBehaviour
    {
        public RewardData testItem;

        public void Additem()
        {
            BagSystemForTower.Instance.AddItem(testItem);
        }
        
    }
