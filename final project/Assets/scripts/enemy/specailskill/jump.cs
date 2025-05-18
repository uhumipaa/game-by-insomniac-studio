using UnityEngine;
using System.Collections;
using UnityEngine.UIElements;

public class Jump : MonoBehaviour,IEnemySpecilskillBehavior
{
    public GameObject shodow;
    public AnimationCurve heightCurve;
    public CircleCollider2D hitbox;
    private BoxCollider2D oghitbox;
    private IEnemySkillContollerInterface controller;
    [SerializeField] float duration;
    [SerializeField] float maxHeight;
    private float scales;
    private Transform Player;
    private enemy_property property;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        property = GetComponent<enemy_property>();
        oghitbox = GetComponent<BoxCollider2D>();
        hitbox.enabled = false;
        hitbox = GetComponent<CircleCollider2D>();
        controller = GetComponent<IEnemySkillContollerInterface>();
    }

    private IEnumerator jump(Vector3 start, Vector3 end,float ogscale)
    {
        oghitbox.enabled = false;
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
            shodow.transform.localPosition =new Vector3(0f, -0.45f, 0f);
            float shadowScale = Mathf.Lerp(0.2f, 0.05f, heightTime);
            shodow.transform.localScale = new Vector3(3*shadowScale, shadowScale, 1f);
            float scale = ogscale + 0.5f * height;
            transform.localScale = new Vector3(scale, scale, 1f);
            yield return null;
        }
        finish();
    }
    public void finish()
    {
        hitbox.enabled = false;
        oghitbox.enabled = true;
        controller.Finishskill();
    }
    public IEnumerator readytojump()
    {
        yield return new WaitForSecondsRealtime(0.3f);
        StartCoroutine(jump(transform.position, Player.position, scales));
    }
    public void usingskill(Transform self, Transform player, enemy_property property, float scale)
    {
        scales = scale;
        Player = player;
        StartCoroutine(readytojump());

    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            collision.GetComponent<Player_Property>().takedamage(property.atk, transform.position);
        }
    }
    // Update is called once per frame
}
