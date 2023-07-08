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

    public enum SpecialAttacks { None, Freeze, Poison, Block, Heal }

    [SerializeField]
    SpecialAttacks specialAttackA = SpecialAttacks.None;

    [SerializeField]
    SpecialAttacks specialAttackB = SpecialAttacks.None;

    //[SerializeField]
    //protected Slider cooldownBar;

    [SerializeField]
    protected int attackDamage = 1;

    private float currentCooldown = 0f;
    private float cooldownTimer = 0f;
    public bool IsCoolingDown => cooldownTimer > 0f;



    public EventReference attackEventPath;
    public EventReference hurtEventPath;
    public EventReference specialEventPath;

    //[SerializeField]
    //protected float attackCoolDown = 3f;
    //protected bool onCoolDown = false;
    //public float coolDownRemaining;

    protected const int maxHealth = 10;
    public int Health { get; protected set; }



    protected Vector3 startPos { get; private set; }
    public bool IsActing { get; protected set; }

    public bool IsDead => Health <= 0;


    protected virtual void Awake()
    {
        Health = maxHealth;
        startPos = transform.position;
    }

    private void Start()
    {
        healthBar.value = Health / (float)maxHealth;
    }

    private void Update()
    {
        if (!IsFrozen)
        {
            cooldownTimer -= Time.deltaTime;
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

        if (Health <= 0)
            Death();

        FMODUnity.RuntimeManager.PlayOneShot(hurtEventPath, gameObject.transform.position);
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
        const float attackCooldown = 3f;

        if (IsActing) return;
        IsActing = true;

        SetCooldown(attackCooldown);

        Vector3 targetPos = Opponent.transform.position;
        MoveToEnemy(startPos, targetPos, .1f,
            () => Opponent.TakeDamage(attackDamage),
            () => IsActing = false);
        FMODUnity.RuntimeManager.PlayOneShot(attackEventPath, gameObject.transform.position);
    }

    public void SpecialAttackA()
    {
        FMODUnity.RuntimeManager.PlayOneShot(specialEventPath, gameObject.transform.position);

        switch (specialAttackA)
        {
            case SpecialAttacks.Freeze:
                SetCooldown(freezeCooldown);
                break;
            case SpecialAttacks.Poison:
                SetCooldown(poisonCooldown);
                break;
            case SpecialAttacks.Block:
                SetCooldown(blockCooldown);
                break;
            case SpecialAttacks.Heal:
                SetCooldown(healCooldown);
                break;
        }
    }

    const float healCooldown = .8f;


    const float freezeCooldown = .4f;
    Timer frozenTimer = new Timer();
    public bool IsFrozen => !frozenTimer.IsExpired;
    public void ApplyFreeze()
    {
        frozenTimer.Set(1f);
    }


    const float poisonCooldown = .4f;
    public bool IsPoisoned { get; private set; } = false;
    public void ApplyPoison()
    {
        StartCoroutine(_PoisonStatus());

        IEnumerator _PoisonStatus()
        {
            IsPoisoned = true;

            for (int i = 0; i < 3; i++)
            {
                yield return new WaitForSeconds(1f);
                TakeDamage(1);
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


    //protected void startCoolDown(float coolDownTime) {
    //    cooldownBar.maxValue = coolDownTime;
    //    cooldownBar.value = coolDownTime;
    //    coolDownRemaining = coolDownTime;
    //    IsActing = false;
    //}

    //private void Update()
    //{
    //    if (!IsActing) {
    //        coolDownRemaining = Mathf.Clamp(coolDownRemaining - Time.deltaTime, 0, cooldownBar.maxValue);
    //        cooldownBar.value = coolDownRemaining;
    //        if (coolDownRemaining <= 0) {
    //            IsActing = true;
    //        }
    //    }
    //}
}
