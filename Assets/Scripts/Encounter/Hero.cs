using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Hero : Fighter
{
    protected override Fighter Opponent => encounterManager.Monster;


    // Hero AI

    private void Awake()
    {
        if (this.cooldownBar == null) {
            this.cooldownBar = GameObject.FindGameObjectWithTag("HeroCooldown").GetComponent<Slider>();
        }
    }

    public IEnumerator _PerformAITurn()
    {
        Debug.Log("Started AI turn");

        SimpleAttack();

        yield return new WaitUntil(() => !IsActing);
        Debug.Log("Ended AI turn");
    }
}
