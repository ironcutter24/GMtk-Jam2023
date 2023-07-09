using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EncounterManager : MonoBehaviour
{
    [SerializeField]
    CombatSelection combatInput;

    [SerializeField]
    Transform monsterStartPos;

    [SerializeField]
    Slider monsterHealthBar;

    [SerializeField]
    Slider monsterCooldownBar;

    [SerializeField]
    TextMeshProUGUI monsterHealthLabel;

    [SerializeField]
    TextMeshProUGUI healthLabel;

    [SerializeField]
    Hero hero;
    public Fighter Hero => hero;

    private Monster monster;
    public Fighter Monster => monster;

    enum TurnOwner { Hero, Monster }
    TurnOwner turnOwner = TurnOwner.Hero;

    bool IsActing => hero.IsActing || monster.IsActing;

    private void Awake()
    {
        monster = Instantiate(GameManager.Instance.CurrentOpponent, monsterStartPos.position, Quaternion.identity);
        combatInput.monster = monster;
        monster.SetBars(monsterHealthBar, monsterCooldownBar, healthLabel);
        monster.SetManager(this);
        hero.SetManager(this);
    }

    private void Update()
    {
        combatInput.SetInteractable(!IsActing && !monster.IsCoolingDown);

        if (IsActing) return;

        if (!hero.IsCoolingDown)
        {
            hero.SimpleAttack();
        }
    }

    public void SetSpecialAttacksUI(Fighter.SpecialAttacks A, Fighter.SpecialAttacks B)
    {
        SetButton(combatInput.specialButtonA, A);
        SetButton(combatInput.specialButtonB, B);

        void SetButton(Button button, Fighter.SpecialAttacks attackType)
        {
            button.GetComponentInChildren<TextMeshProUGUI>().text = attackType.ToString();
            button.gameObject.SetActive(attackType != Fighter.SpecialAttacks.None);
        }
    }
}
