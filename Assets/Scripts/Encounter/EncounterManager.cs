using System.Collections;
using System.Collections.Generic;
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

    //[SerializeField]
    //Slider monsterCooldownBar;

    [SerializeField]
    Hero hero;
    public Fighter Hero => hero;

    private Monster monster;
    public Fighter Monster => monster;

    enum TurnOwner { Hero, Monster }
    TurnOwner turnOwner = TurnOwner.Hero;

    private void Awake()
    {
        monster = Instantiate(GameManager.Instance.CurrentOpponent, monsterStartPos.position, Quaternion.identity);
        combatInput.monster = monster;
        monster.SetHealthBar(monsterHealthBar);
        //monster.SetCooldownBar(monsterCooldownBar);
        monster.SetManager(this);
        hero.SetManager(this);
    }

    private void Start()
    {
        StartCoroutine(_TestFight());
    }

    IEnumerator _TestFight()
    {
        yield return new WaitForSeconds(1.2f);

        while (true)
        {
            if (turnOwner == TurnOwner.Monster)
            {
                // Player turn
                yield return _WaitForPlayerTurn();

                turnOwner = TurnOwner.Hero;
            }
            else
            {
                // AI turn
                yield return hero._PerformAITurn();
                turnOwner = TurnOwner.Monster;
            }
        }
    }

    IEnumerator _WaitForPlayerTurn()
    {
        Debug.Log("Started player turn");

        combatInput.SetInteractable(true);
        yield return new WaitUntil(() => monster.IsActing);

        combatInput.SetInteractable(false);
        yield return new WaitUntil(() => !monster.IsActing);

        Debug.Log("Ended player turn");
    }
}
