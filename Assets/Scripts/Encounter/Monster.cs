using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;

public class Monster : Fighter
{
    protected override Fighter Opponent => encounterManager.Hero;

    public void SetHealthBar(Slider bar)
    {
        healthBar = bar;
    }
}