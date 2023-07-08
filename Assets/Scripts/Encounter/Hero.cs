using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hero : Fighter
{
    protected override Fighter Opponent => encounterManager.Monster;


    // Hero AI

    public IEnumerator _PerformAITurn()
    {
        Debug.Log("Started AI turn");

        SimpleAttack();

        yield return new WaitUntil(() => !IsActing);
        Debug.Log("Ended AI turn");
    }
}
