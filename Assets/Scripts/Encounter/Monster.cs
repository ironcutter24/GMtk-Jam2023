using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster : Fighter
{
    public override void SimpleAttack()
    {
        IsActing = true;

        Vector3 targetPos = encounterManager.Hero.transform.position;
        MoveToEnemy(startPos, targetPos, attackMoveSpeed, .05f,
            () => IsActing = false);
    }
}
