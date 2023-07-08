using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;


public class Monster : Fighter
{
    protected override Fighter Opponent => encounterManager.Hero;

    protected override void Start()
    {
        base.Start();
        encounterManager.SetSpecialAttacksUI(specialAttackA, specialAttackB);
    }

    public void SetBars(Slider health, Slider cooldown)
    {
        healthBar = health;
        cooldownBar = cooldown;
    }

    protected override void Death()
    {
        GameManager.Instance.Hero.SetHealth(Opponent.Health);
        GameManager.Instance.LoadWorldMap();
    }

}
