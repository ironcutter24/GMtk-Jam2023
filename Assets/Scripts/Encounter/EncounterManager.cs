using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EncounterManager : MonoBehaviour
{
    [SerializeField]
    Hero hero;
    public Fighter Hero => hero;

    [SerializeField]
    Monster monster;
    public Fighter Monster => monster;

    enum TurnOwner { Hero, Monster }
    TurnOwner turnOwner = TurnOwner.Hero;

    private void Awake()
    {
        hero.SetManager(this);
        monster.SetManager(this);

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


                turnOwner = TurnOwner.Monster;
            }

            yield return null;
        }
    }
}
