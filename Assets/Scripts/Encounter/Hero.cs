using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Hero : Fighter
{
    protected override Fighter Opponent => encounterManager.Monster;

    protected override void Awake()
    {
        base.Awake();
        GameManager.Instance.Hero.SetHealth(Health);
    }

    // Hero AI

    //private void Awake()
    //{
    //    if (this.cooldownBar == null) {
    //        this.cooldownBar = GameObject.FindGameObjectWithTag("HeroCooldown").GetComponent<Slider>();
    //    }
    //}

    public IEnumerator _PerformAITurn()
    {
        Debug.Log("Started AI turn");

        SimpleAttack();

        yield return new WaitUntil(() => !IsActing);
        Debug.Log("Ended AI turn");
    }

    protected override void Death()
    {
        // To win screen
    }
}
