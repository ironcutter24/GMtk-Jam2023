using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster : Fighter
{
    protected override Fighter Opponent => encounterManager.Hero;

    protected override void Start()
    {
        base.Start();

        //SimpleAttack();
    }
}
