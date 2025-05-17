using UnityEngine;
using Flower;
public class IntroSenceController : MonoBehaviour
{
    FlowerSystem fs;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //設定並載入intro
        fs = FlowerManager.Instance.CreateFlowerSystem("default",false);
        fs.SetupDialog();
        fs.ReadTextFromResource("intro");
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            fs.Next();
        }
    }
}
