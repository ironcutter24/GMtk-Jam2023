using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatLocation : MonoBehaviour
{
    [SerializeField]
    Monster opponent;

    public void InitBattle()
    {
        GameManager.Instance.LoadBattleWith(opponent);
    }
}
