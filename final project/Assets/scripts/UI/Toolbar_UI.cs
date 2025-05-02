using System.Collections.Generic;
using UnityEngine;

public class Toolbar_UI : MonoBehaviour
{
   [SerializeField] private List <Slot_UI> toolbarSlots = new List<Slot_UI>();

    private Slot_UI selectedSlot;

    private void Start()
    {
        SelectSlot(0); //一開始時選擇第一格
    }

    private void Update()
    {
        CheckAlphaNumericKeys();
    }

    public void SelectSlot(int index)
    {

        if(toolbarSlots.Count == 10) //若有10個欄位時
        {
            if(selectedSlot != null)
            {
                selectedSlot.SetHighlight(false); //把前一個highlight關掉
            }
            selectedSlot = toolbarSlots[index];
            selectedSlot.SetHighlight(true); //重新選定後打開highlight
        }
    }

    private void CheckAlphaNumericKeys()
    {
        //按下數字1 選取第0格
        if(Input.GetKeyDown(KeyCode.Alpha1))
        {
            SelectSlot(0);
        }

        if(Input.GetKeyDown(KeyCode.Alpha2))
        {
            SelectSlot(1);
        }

        if(Input.GetKeyDown(KeyCode.Alpha3))
        {
            SelectSlot(2);
        }

        if(Input.GetKeyDown(KeyCode.Alpha4))
        {
            SelectSlot(3);
        }

        if(Input.GetKeyDown(KeyCode.Alpha5))
        {
            SelectSlot(4);
        }

        if(Input.GetKeyDown(KeyCode.Alpha6))
        {
            SelectSlot(5);
        }

        if(Input.GetKeyDown(KeyCode.Alpha7))
        {
            SelectSlot(6);
        }

        if(Input.GetKeyDown(KeyCode.Alpha8))
        {
            SelectSlot(7);
        }

        if(Input.GetKeyDown(KeyCode.Alpha9))
        {
            SelectSlot(8);
        }

        if(Input.GetKeyDown(KeyCode.Alpha0))
        {
            SelectSlot(9);
        }

    }
}
