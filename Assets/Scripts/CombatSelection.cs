using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CombatSelection : MonoBehaviour
{
    public Monster monster;
    public Slider monsterHealthSlider;
    public Slider heroHealthSlider;

    [SerializeField]
    private List<Button> buttons;

    public Button specialButtonA;
    public Button specialButtonB;

    private void Awake()
    {
        SetInteractable(false);
    }

    public void SetInteractable(bool state)
    {
        foreach (var button in buttons)
            button.interactable = state;
    }


    public void Attack()
    {
        Debug.Log("Attack Button Pressed");
        monster.SimpleAttack();
    }

    public void Block()
    {
        Debug.Log("Block Button pressed");
        monster.Block();
    }

    public void SpecialA()
    {
        Debug.Log("Special (A) Button Pressed");
        monster.SpecialAttackA();
    }

    public void SpecialB()
    {
        Debug.Log("Special (B) Button Pressed");
        monster.SpecialAttackB();
    }

}
