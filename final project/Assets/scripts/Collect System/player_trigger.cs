using UnityEngine;

public class player_trigger : MonoBehaviour
{
    public Inventory inventory;
    private void Awake()
    {
        inventory = new Inventory(21);   
    }

}
