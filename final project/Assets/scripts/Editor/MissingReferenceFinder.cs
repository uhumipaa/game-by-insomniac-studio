using UnityEngine;
using UnityEditor;

public class FindBrokenComponents
{
    [MenuItem("Tools/找出壞掉的組件")]
    static void FindBroken()
    {
        GameObject[] all = GameObject.FindObjectsOfType<GameObject>();
        foreach (GameObject g in all)
        {
            Component[] components = g.GetComponents<Component>();
            foreach (var c in components)
            {
                if (c == null)
                {
                    Debug.LogWarning("❌ 壞掉組件在 → " + GetFullPath(g), g);
                } 
            }
        }
    }

    static string GetFullPath(GameObject g)
    {
        string path = g.name;
        while (g.transform.parent != null)
        {
            g = g.transform.parent.gameObject;
            path = g.name + "/" + path;
        }
        return path;
    }
}