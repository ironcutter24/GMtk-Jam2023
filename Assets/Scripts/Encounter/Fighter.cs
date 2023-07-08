using DG.Tweening;
using FMODUnity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Utility.Time;

public abstract class Fighter : MonoBehaviour
{
    protected abstract Fighter Opponent { get; }

    protected const float attackMoveSpeedIn = 42f;
    protected const float attackMoveSpeedOut = 16f;
    protected EncounterManager encounterManager;

    [SerializeField]
    protected Slider healthBar;
    [SerializeField]
    protected Slider cooldownBar;

    [SerializeField]
    private SpriteRenderer spriteRenderer;

    public enum SpecialAttacks { None, Freeze, Poison, Block, Heal }

    [SerializeField]
    protected SpecialAttacks specialAttackA = SpecialAttacks.None;

    [SerializeField]
    protected SpecialAttacks specialAttackB = SpecialAttacks.None;


    private float currentCooldown = 0f;
    private float cooldownTimer = 0f;
    public bool IsCoolingDown => cooldownTimer > 0f;


    public EventReference attackEventPath;
    public EventReference hurtEventPath;
    public EventReference specialEventPath;

    //Parameters that we can tweak on the individual fighters
    [SerializeField]
    protected int maxHealth = 10;
    [SerializeField]
    protected float attackCooldown = 3f;
    [SerializeField]
    protected int attackDamage = 1;
    [SerializeField]
    protected float healCooldown = .8f;
    [SerializeField]
    protected float freezeCooldown = .4f;
    [SerializeField]
    protected float poisonCooldown = .4f;
    [SerializeField]
    protected float freezeTime = 1f;
    [SerializeField]
    protected int poisonDamage = 1;
    [SerializeField]
    protected int poisonTurns = 3;
    [SerializeField]
    protected float poisonDelay = 1f;

    public int Health { get; protected set; }


    protected Vector3 startPos { get; private set; }
    public bool IsActing { get; protected set; }

    public bool IsDead => Health <= 0;


    protected virtual void Awake()
    {
        Health = maxHealth;
        startPos = transform.position;
    }

    protected virtual void Start()
    {
        healthBar.value = Health / (float)maxHealth;
    }

    private void Update()
    {
        if (!IsFrozen)
        {
            cooldownTimer -= Time.deltaTime;
            cooldownBar.value = cooldownTimer / currentCooldown;
        }
    }

    void SetCooldown(float time)
    {
        cooldownTimer = currentCooldown = time;
    }

    public void SetManager(EncounterManager encounterManager)
    {
        this.encounterManager = encounterManager;
    }

    public void TakeDamage(int damage)
    {
        Health = Mathf.Max(0, Health - damage);
        healthBar.value = Health / (float)maxHealth;
        
        FMODUnity.RuntimeManager.PlayOneShot(hurtEventPath, gameObject.transform.position);
        transform.DOShakePosition(.3f, .5f, 30);
        //spriteRenderer.material.SetFloat("HITEFFECT_ON", 1f);


        if (Health <= 0)
            Death();
    }

    protected abstract void Death();


    protected void MoveToEnemy(Vector3 from, Vector3 to, float hitStop, System.Action OnReach, System.Action OnComplete)
    {
        float dist = Vector2.Distance(from, to);
        Sequence attackTween = DOTween.Sequence();
        attackTween
            .Append(transform.DOMove(to, dist / attackMoveSpeedIn).SetEase(Ease.Linear))
            .AppendCallback(() => OnReach())
            .AppendInterval(hitStop)
            .Append(transform.DOMove(from, dist / attackMoveSpeedOut))
            .OnComplete(() => OnComplete());
    }


    public void SimpleAttack()
    {
        if (IsActing) return;
        IsActing = true;

        SetCooldown(attackCooldown);

        Vector3 targetPos = Opponent.transform.position;
        MoveToEnemy(startPos, targetPos, .1f,
            () => Opponent.TakeDamage(attackDamage),
            () => IsActing = false);
        FMODUnity.RuntimeManager.PlayOneShot(attackEventPath, gameObject.transform.position);
    }

    public void Block()
    {

    }

    public void SpecialAttackA()
    {
        FMODUnity.RuntimeManager.PlayOneShot(specialEventPath, gameObject.transform.position);

        switch (specialAttackA)
        {
            case SpecialAttacks.Freeze:
                SetCooldown(freezeCooldown);
                Opponent.ApplyFreeze();
                break;
            case SpecialAttacks.Poison:
                SetCooldown(poisonCooldown);
                Opponent.ApplyPoison();
                break;
            case SpecialAttacks.Block:
                SetCooldown(blockCooldown);
                break;
            case SpecialAttacks.Heal:
                SetCooldown(healCooldown);
                break;
        }
    }

    public void SpecialAttackB()
    {
        FMODUnity.RuntimeManager.PlayOneShot(specialEventPath, gameObject.transform.position);

        switch (specialAttackA)
        {
            case SpecialAttacks.Freeze:
                SetCooldown(freezeCooldown);
                Opponent.ApplyFreeze();
                break;
            case SpecialAttacks.Poison:
                SetCooldown(poisonCooldown);
                Opponent.ApplyPoison();
                break;
            case SpecialAttacks.Block:
                SetCooldown(blockCooldown);
                break;
            case SpecialAttacks.Heal:
                SetCooldown(healCooldown);
                break;
        }
    }

    Timer frozenTimer = new Timer();
    public bool IsFrozen => !frozenTimer.IsExpired;
    public void ApplyFreeze()
    {
        frozenTimer.Set(freezeTime);
    }


    public bool IsPoisoned { get; private set; } = false;
    public void ApplyPoison()
    {
        StartCoroutine(_PoisonStatus());

        IEnumerator _PoisonStatus()
        {
            IsPoisoned = true;

            for (int i = 0; i < poisonTurns; i++)
            {
                yield return new WaitForSeconds(poisonDelay);
                TakeDamage(poisonDamage);
            }

            IsPoisoned = false;
        }
    }


    const float blockCooldown = .4f;
    Timer blockignTimer = new Timer();
    public bool IsBlocking { get; private set; } = false;
    public void ApplyBlocking()
    {
        blockignTimer.Set(1f);
    }

}
