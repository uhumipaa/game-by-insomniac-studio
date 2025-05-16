using UnityEngine;

public class Enemy_close_Animation : MonoBehaviour,IEnemyAnimatorBehavior
{
    private IEnemyControllerInterface controller;

    void Awake()
    {
        controller = GetComponent<IEnemyControllerInterface>();
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public void PlayAttack(Vector2 direction, Animator ani)
    {
        if (Mathf.Abs(direction.y) < 0.2f)
        {
            ani.SetFloat("horizontal", 1f);
            ani.SetFloat("vertical", 0f);
        }
        else
        {
            float verticalparameter = direction.y > 0f ? 1f : -1f;
            ani.SetFloat("horizontal", 0f);
            ani.SetFloat("vertical", verticalparameter);
        }
        ani.SetBool("attacking", true);
        ani.SetBool("waiting", false);
        ani.SetBool("running", false);
    }
    public void PlayMove(Vector2 direction, Animator ani)
    {
        if (Mathf.Abs(direction.y) < 0.2f)
        {
            ani.SetFloat("horizontal", 1f);
            ani.SetFloat("vertical", 0f);
        }
        else
        {
            float verticalparameter = direction.y > 0f ? 1f : -1f;
            ani.SetFloat("horizontal", 0f);
            ani.SetFloat("vertical", verticalparameter);
        }
        ani.SetBool("attacking", false);
        ani.SetBool("waiting", false);
        ani.SetBool("running", true);
    }
    public void PlayIdle(Animator ani)
    {
        ani.SetBool("attacking", false);
        ani.SetBool("waiting", true);
        ani.SetBool("running", false);
    }

    public void NotifyAttackFinished(Animator ani)
    {
        Debug.Log("finish");
        controller.FinishAttack();
    }
}
