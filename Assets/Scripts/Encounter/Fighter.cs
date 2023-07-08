using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Fighter : MonoBehaviour
{
    protected const float attackMoveSpeed = 16f;

    protected EncounterManager encounterManager;

    protected Vector3 startPos { get; private set; }

    public bool IsActing { get; protected set; }

    private void Start()
    {
        startPos = transform.position;

        SimpleAttack();
    }

    public void SetManager(EncounterManager encounterManager)
    {
        this.encounterManager = encounterManager;
    }

    public abstract void SimpleAttack();

    protected void MoveToEnemy(Vector3 from, Vector3 to, float moveSpeed, float hitStop, System.Action callback)
    {
        float transitionTime = Vector2.Distance(from, to) / moveSpeed;
        Sequence attackTween = DOTween.Sequence();
        attackTween
            .Append(transform.DOMove(to, transitionTime))
            .AppendInterval(hitStop)
            .Append(transform.DOMove(from, transitionTime))
            .OnComplete(() => callback());
    }
}
