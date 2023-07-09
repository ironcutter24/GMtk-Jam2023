using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hero : Fighter
{
    protected override Fighter Opponent => encounterManager.Monster;

    public GameObject victoryScreen;

    protected override void Awake()
    {
        base.Awake();
        victoryScreen.SetActive(false);

        Health = GameManager.Instance.Hero.Health;
        maxHealth = GameManager.Instance.Hero.MaxHealth;
        attackDamage = GameManager.Instance.Hero.AttackDamage;
    }

    protected override void Death()
    {
        Time.timeScale = 0;
        victoryScreen.SetActive(true);
    }
}
