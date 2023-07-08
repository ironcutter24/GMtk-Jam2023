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

    [SerializeField]
    Hero hero;
    public Fighter Hero => hero;

    public Fighter Monster { get; private set; }

    enum TurnOwner { Hero, Monster }
    TurnOwner turnOwner = TurnOwner.Hero;

    private void Awake()
    {
        Monster = Instantiate(GameManager.Instance.CurrentOpponent, monsterStartPos.position, Quaternion.identity);
        combatInput.monster = Monster as Monster;
        Monster.SetManager(this);

        hero.SetManager(this);

        StartCoroutine(_TestFight());
    }

    IEnumerator _TestFight()
    {
        while (true)
        {
            if (turnOwner == TurnOwner.Monster)
            {
                // Wait for player input

                turnOwner = TurnOwner.Hero;
            }
            else
            {
                yield return new WaitForSeconds(.4f);

                turnOwner = TurnOwner.Monster;
            }

            yield return null;
        }
    }
}
