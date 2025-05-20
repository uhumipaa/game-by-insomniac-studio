using UnityEngine;
using UnityEngine.UI;

public class WaitingFishAnimationCustom : MonoBehaviour
{
    public Sprite[] sprites; // 拖入 WaitingText0~3（順序不限）
    public float frameDelay = 0.6f; // 切換間隔
    public float totalDuration = 8f; // 總播放時間

    public GameObject linkedFishPic; // 要一起關掉的圖片（WaitingFishPic）

    private Image image;
    private float timer = 0f;
    private float totalTimer = 0f;
    private int[] sequence = new int[] { 2, 3, 1, 2, 3, 1 }; // 自訂順序
    private int sequenceIndex = 0;
    private bool started = false;

    void OnEnable()
    {
        image = GetComponent<Image>();
        sequenceIndex = 0;
        timer = 0f;
        totalTimer = 0f;
        started = true;

        if (sprites.Length > 0)
            image.sprite = sprites[0]; // 一開始顯示 WaitingText0
    }

    void Update()
    {
        if (!started) return;

        totalTimer += Time.deltaTime;
        timer += Time.deltaTime;

        if (totalTimer >= totalDuration)
        {
            // 時間到，關掉自己跟 linkedFishPic
            gameObject.SetActive(false);
            if (linkedFishPic != null)
                linkedFishPic.SetActive(false);

            started = false;
            return;
        }

        if (timer >= frameDelay)
        {
            timer = 0f;
            int spriteIndex = sequence[sequenceIndex % sequence.Length];
            if (spriteIndex < sprites.Length)
            {
                image.sprite = sprites[spriteIndex];
            }
            sequenceIndex++;
        }
    }
}
