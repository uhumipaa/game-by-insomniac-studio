using UnityEngine;

public class deathnow : MonoBehaviour
{
    private Player_Property Player_Property;
    private Vector2 o;
    public void death()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            Player_Property = player.GetComponent<Player_Property>();
        }
        Player_Property.takedamage(100000, o);
    }
}
