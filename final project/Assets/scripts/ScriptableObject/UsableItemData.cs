using UnityEngine;

[CreateAssetMenu(fileName = "UsableItemData", menuName = "Scriptable Objects/UsableItemData")]
public abstract class UsableItemData : ScriptableObject
{
    

    public abstract void OnUse(Player_Property player);
}
