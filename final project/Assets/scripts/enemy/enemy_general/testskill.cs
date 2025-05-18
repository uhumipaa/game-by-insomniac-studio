using UnityEngine;

public class testskill : MonoBehaviour,IEnemySkillContollerInterface
{
    private Animator ani;
    public int skillcount;
    private IEnemySpecilskillBehavior skill;
    public MonoBehaviour script;
    public Transform player;
    private enemy_property property;
    [SerializeField] float scale;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        ani = GetComponent<Animator>();
        skill = script as IEnemySpecilskillBehavior;
        property = GetComponent<enemy_property>();
        use();
    }

    // Update is called once per frame
    void use()
    {
        switch (skillcount)
        {
            case 0:
                ani.SetBool("dashing", true);
                break;
            case 1:
                ani.SetBool("jumping", true);
                break;
            case 2:
                ani.SetBool("casting", true);
                break;
        }
        skill.usingskill(transform, player, property, scale);
    }
    public void Finishskill()
    {
        Debug.Log("finish");
        ani.SetBool("waiting", true);
    }
}
