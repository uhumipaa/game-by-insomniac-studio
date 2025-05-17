using UnityEngine;
using System.Collections;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using static UnityEditor.ShaderGraph.Internal.KeywordDependentCollection;
public class FireStone_controller: MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [Header("¼Ò²Õ")]
    public GameObject hint;
    public GameObject firestone;
    public GameObject explosion;
    private Rigidbody2D stonerb;

    [Header("ÄÝ©Ê")]
    [SerializeField] private AnimationCurve dashSpeedCurve = AnimationCurve.EaseInOut(0, 0, 1, 1);
    [SerializeField] float speed;

    private bool meet=false;
    // Update is called once per frame
    private void Awake()
    {
        stonerb = firestone.GetComponent<Rigidbody2D>();
        hint.SetActive(false);
        firestone.SetActive(false);
        explosion.SetActive(false);
    }
    public void Setdrop(Vector2 position)
    {
        firestone.SetActive(true);
        hint.SetActive(true);
        hint.transform.position = position;
        hint.transform.localScale = Vector3.zero;
        firestone.transform.position = position + new Vector2(-10f, 10f);
        StartCoroutine(fall(hint.transform.position));
    }
    public IEnumerator fall(Vector2 position)
    {
        Vector2 start = firestone.transform.position;
        float totaldistance = Vector2.Distance(position, start);
        Vector2 direction = (position - (Vector2)firestone.transform.position).normalized;
        while (!meet)
        {
            float distance = Vector2.Distance(position, firestone.transform.position);
            float t = 1f - distance / totaldistance;        
            float speedFactor = dashSpeedCurve.Evaluate(t);
            hint.transform.localScale = new Vector3(2 * t+0.1f, t+0.1f, 1f);
            stonerb.linearVelocity = direction*speed*speedFactor;
            //firestone.transform.position = Vector2.MoveTowards(firestone.transform.position,position,speed * Time.deltaTime);
            Debug.Log($"t: {t}, scale: {hint.transform.localScale}, pos: {firestone.transform.position}");
            yield return null;
        }
        stonerb.linearVelocity = Vector2.zero;
        bomb(position);
    }
    public void bomb(Vector2 position)
    {
        hint.SetActive(false);
        firestone.SetActive(false);
        explosion.SetActive(true);
        explosion.transform.position = position;
    }

    public void Meet()
    {
        meet = true;
    }
        
}
