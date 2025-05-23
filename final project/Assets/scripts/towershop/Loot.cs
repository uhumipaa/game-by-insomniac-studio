using UnityEngine;

public class Loot : MonoBehaviour
{
    public SpriteRenderer sr;
    public Animator ani;
    public ItemData itemdata;

    void OnValidate()
    {
        if (itemdata == null) return;
        sr.sprite = itemdata.icon;
        this.name = itemdata.name;
    }
    public void updateloot()
    {
        if (itemdata == null) return;
        sr.sprite = itemdata.icon;
        this.name = itemdata.name;
    }
}
