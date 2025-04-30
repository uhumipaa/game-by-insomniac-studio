using UnityEngine;

public class player_trigger : MonoBehaviour
{
    public Inventory inventory;
    private void Awake()
    {
        Debug.Log("【Awake】初始化 Inventory 成功！物件：" + this.gameObject.name);
        inventory = new Inventory(28);   
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Backspace))
        {
            Vector3Int position = new Vector3Int((int)transform.position.x, (int)transform.position.y, 0);

            if(GameManager.instance.tileManager.IsInteractable(position))
            {
                Debug.Log("Tile is interactable");
            }
        }
    }
}
