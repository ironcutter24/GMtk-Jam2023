using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class Monster : Fighter
{
    protected override Fighter Opponent => encounterManager.Hero;

    [Header("World Map")]
    [SerializeField]
    Sprite iconSprite;
    public Sprite IconSprite => iconSprite;

    [SerializeField]
    Color iconColor = Color.white;
    public Color IconColor => iconColor;

    protected override void Start()
    {
        base.Start();
        encounterManager.SetSpecialAttacksUI(specialAttackA, specialAttackB);
    }

    public void SetBars(Slider health, Slider cooldown, TextMeshProUGUI label)
    {
        healthBar = health;
        cooldownBar = cooldown;
        healthLabel = label;
    }

    protected override void Death()
    {
        GameManager.Instance.Hero.SetHealth(Opponent.Health);
        GameManager.Instance.LoadWorldMap();
        Debug.Log("Monster Dead");
    }

}
