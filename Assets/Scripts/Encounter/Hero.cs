using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Hero : Fighter
{
    protected override Fighter Opponent => encounterManager.Monster;
    public override int AttackDamage => GameManager.Instance.Hero.AttackDamage;

    public GameObject victoryScreen;

    protected override void Awake()
    {
        base.Awake();
        Health = GameManager.Instance.Hero.Health;
        if (Health > 0)
        {
            victoryScreen.SetActive(false);
        }
        else
        {
            Death();
        }
    }

    // Hero AI

    public IEnumerator _PerformAITurn()
    {
        Debug.Log("Started AI turn");

        SimpleAttack();

        yield return new WaitUntil(() => !IsActing);
        Debug.Log("Ended AI turn");
    }

    protected override void Death()
    {
        Time.timeScale = 0;
        victoryScreen.SetActive(true);
    }
}
