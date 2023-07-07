using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Fighter : MonoBehaviour
{
    protected const float attackMoveSpeed = 16f;

    protected EncounterManager encounterManager;

    protected Vector3 startPos { get; private set; }

    public bool IsActing { get; private set; }

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
    public abstract void Block();
}
