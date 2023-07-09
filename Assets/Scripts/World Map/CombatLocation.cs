using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatLocation : MonoBehaviour
{
    [SerializeField]
    Monster opponent;

    private void Start()
    {
        if (opponent == null)
        {
            throw new NullReferenceException();
        }

        GetComponent<WorldLocation>().SetIcon(opponent.IconSprite, opponent.IconColor);
    }

    public void InitBattle()
    {
        GameManager.Instance.LoadBattleWith(opponent);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.magenta;
        Gizmos.DrawSphere(transform.position, .2f);
    }
}
