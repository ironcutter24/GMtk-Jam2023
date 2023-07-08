using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hero : Fighter
{
    // Hero AI

    public override void SimpleAttack()
    {
        IsActing = true;

        Vector3 targetPos = encounterManager.Monster.transform.position;
        MoveToEnemy(startPos, targetPos, attackMoveSpeed, .05f,
            () => IsActing = false);
    }
}
