using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class ChainLighting : MonoBehaviour,isMagic
{
    public int chainnumber;
    public float range;
    public float dmgpara;
    private Player_Property property;
    public float chiandelay;
    public GameObject lightingprefab;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        property = GetComponent<Player_Property>();
    }
    public void cast()
    {
        StartCoroutine(Lighting(transform.position, new List<GameObject>(), chainnumber));
    }
    private IEnumerator Lighting(Vector3 postion,List<GameObject> hitenemy, int remainingJumps)
    {
        if (remainingJumps <= 0)
        {
            yield break;
        }
        GameObject target = GetComponent<Getclosestenemy>().getclosest(transform.position,hitenemy,range);
        if (target != null)
        {
            createlighting(postion, target.transform.position);
            hitenemy.Add(target);
            target.GetComponent<enemy_property>()?.takedamage(property.magic_atk , Vector2.zero);
        }
        else
        {
            yield break;
        }

            yield return new WaitForSeconds(chiandelay);
        StartCoroutine(Lighting(target.transform.position,hitenemy, remainingJumps-1));

    }
    void createlighting(Vector3 form_pos,Vector3 target_pos)
    {
        Debug.Log("CreateLightningEffect called: " + form_pos + " -> " + target_pos);
        Audio_manager.Instance.Play(16, "player_lighting", false, 0);
        GameObject lighting = Instantiate(lightingprefab);
        LineRenderer line = lighting.GetComponent<LineRenderer>();
        line.positionCount = 2;
        line.SetPosition(0, form_pos);
        line.SetPosition(1, target_pos);
        line.startWidth = 0.1f;
        line.endWidth = 0.1f;
        line.widthMultiplier = 1.0f;
        line.material = new Material(Shader.Find("Unlit/Color"));
        AnimationCurve curve = new AnimationCurve();
        curve.AddKey(0.0f, 0.1f);
        curve.AddKey(0.5f, 0.3f);
        curve.AddKey(1.0f, 0.1f);
        line.widthCurve = curve;
        Gradient gradient = new Gradient();
        gradient.SetKeys(
            new GradientColorKey[] {
            new GradientColorKey(new Color(0.6f, 0.9f, 1f), 0.0f),
            new GradientColorKey(new Color(1f, 1f, 1f), 1.0f)
            },
            new GradientAlphaKey[] {
            new GradientAlphaKey(0.0f, 0.0f),
            new GradientAlphaKey(0.7f, 0.5f),
            new GradientAlphaKey(0.0f, 1.0f)
            }
        );
        line.colorGradient = gradient;
        Destroy(lighting, 0.2f);

    }
}
