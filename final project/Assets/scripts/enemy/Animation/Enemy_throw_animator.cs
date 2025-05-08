using UnityEngine;

public class Enemy_throw_animator : MonoBehaviour,IEnemyAnimatorBehavior
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private Enemy_throw_controller controller;

    void Awake()
    {
        controller = GetComponent<Enemy_throw_controller>();
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public void PlayAttack(Vector2 direction, Animator ani)
    {
        ani.SetBool("attacking", true);
        ani.SetBool("waiting", false);
        ani.SetBool("running", false);
    }
    public void PlayMove(Vector2 direction, Animator ani)
    {
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
