using UnityEngine;
using System.Collections;
using UnityEngine.UIElements;

public class Jump : MonoBehaviour,IEnemySpecilskillBehavior
{
    public AnimationCurve heightCurve;
    public CircleCollider2D hitbox;
    private Boss_dino_controller controller;
    [SerializeField] float duration;
    [SerializeField] float maxHeight;
    private float scales;
    private Transform Player;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        hitbox.enabled = false;
        hitbox = GetComponent<CircleCollider2D>();
    }

    private IEnumerator jump(Vector3 start, Vector3 end,float ogscale)
    {
        float timepast = 0f;
        hitbox.enabled = true;
        while (timepast < duration)
        {
            if (transform.position == end)
            {
                break;
            }
            timepast += Time.deltaTime;
            var linearTime = timepast / duration;
            var heightTime = heightCurve.Evaluate(linearTime);
            var height = Mathf.Lerp(0f, maxHeight, heightTime);
            transform.position = Vector3.Lerp(start, end, linearTime) + new Vector3(0f, height, 0f);
            float scale = ogscale + 0.5f * height;
            transform.localScale = new Vector3(scale, scale, 1f);
            yield return null;
        }
        hitbox.enabled = false;
        controller.Finishskill();
    }
    public void jump()
    {
        StartCoroutine(jump(transform.position, Player.position, scales));
    }
    public void usingskill(Transform self, Transform player, enemy_property property, float scale)
    {
        scales = scale;
        Player = player;
    }

    // Update is called once per frame
}
