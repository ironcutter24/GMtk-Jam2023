using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster : Fighter
{
    public override void SimpleAttack()
    {
        Vector3 targetPos = encounterManager.Hero.transform.position;

        float transitionTime = Vector2.Distance(targetPos, startPos) / attackMoveSpeed;

        Sequence attackTween = DOTween.Sequence();
        attackTween
            .Append(transform.DOMove(targetPos, transitionTime))
            .AppendInterval(.05f)
            .Append(transform.DOMove(startPos, transitionTime));
    }

    public override void Block()
    {
        
    }
}
