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

    [Header("Audio")]
    public EventReference attackEventPath;
    public EventReference hurtEventPath;
    public EventReference specialEventPath;

    [Header("Components")]
    [SerializeField]
    protected Slider healthBar;
    [SerializeField]
    protected Slider cooldownBar;

    [SerializeField]
    private SpriteRenderer spriteRenderer;

    public enum SpecialAttacks { None, Freeze, Poison, Block, Heal }

    [Header("Stats")]
    [SerializeField]
    protected int maxHealth = 10;

    [SerializeField]
    protected SpecialAttacks specialAttackA = SpecialAttacks.None;
    [SerializeField]
    protected SpecialAttacks specialAttackB = SpecialAttacks.None;


    //Parameters that we can tweak on the individual fighters

    [Header("Attack")]
    [SerializeField]
    protected float attackCooldown = 3f;
    [SerializeField]
    protected int attackDamage = 1;

    [Header("Block")]
    [SerializeField]
    protected float blockCooldown = .4f;

    [Header("Healing")]
    [SerializeField]
    protected float healCooldown = .8f;
    [SerializeField]
    protected int healAmount = 5;

    [Header("Freeze attack")]
    [SerializeField]
    protected float freezeCooldown = .4f;
    [SerializeField]
    protected float freezeTime = 1f;

    [Header("Poison attack")]
    [SerializeField]
    protected float poisonCooldown = .4f;
    [SerializeField]
    protected int poisonDamage = 1;
    [SerializeField]
    protected int poisonTurns = 3;
    [SerializeField]
    protected float poisonDelay = 1f;

    public int Health { get; protected set; }

    private float currentCooldown = 0f;
    private float cooldownTimer = 0f;
    public bool IsCoolingDown => cooldownTimer > 0f;

    protected Vector3 startPos { get; private set; }
    public bool IsActing { get; protected set; }

    public bool IsDead => Health <= 0;
    protected abstract void Death();


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


    #region Abilities

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

    public bool IsBlocking { get; private set; } = false;
    public void Block()
    {
        IsBlocking = true;

        SetCooldown(blockCooldown);

        DOTween.To(() => 0f, (x) => SetShineLocation(x), 1f, .9f)
            .SetLoops(3, LoopType.Restart)
            .OnComplete(() => IsBlocking = false);

        void SetShineLocation(float val) { spriteRenderer.material.SetFloat("_ShineLocation", val); }
        FMODUnity.RuntimeManager.PlayOneShot("event:/PrepareBlock");
    }

    void Heal()
    {
        SetCooldown(healCooldown);
        Health = Mathf.Clamp(Health + healAmount, 0, maxHealth);
    }

    public void SpecialAttackA()
    {
        ApplySpecialAttack(specialAttackA);
    }

    public void SpecialAttackB()
    {
        ApplySpecialAttack(specialAttackB);
    }

    void ApplySpecialAttack(SpecialAttacks attackType)
    {
        FMODUnity.RuntimeManager.PlayOneShot(specialEventPath, gameObject.transform.position);

        switch (attackType)
        {
            case SpecialAttacks.Freeze:
                SetCooldown(freezeCooldown);
                Opponent.ApplyFreeze();
                break;
            case SpecialAttacks.Poison:
                SetCooldown(poisonCooldown);
                Opponent.ApplyPoison();

                DOTween.To(() => 0f, (x) => spriteRenderer.material.SetFloat("_HsvShift", x), 360f, 1.2f);

                break;
            case SpecialAttacks.Block:
                Block();
                break;
            case SpecialAttacks.Heal:
                Heal();
                break;
        }
    }

    #endregion


    #region Status Effects

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

                DOTween.To(() => 0f, (x) => SetDistorsion(x), .5f, .8f)
                    .SetLoops(1, LoopType.Yoyo);

                TakeDamage(poisonDamage);
            }

            IsPoisoned = false;
        }

        void SetDistorsion(float val) { spriteRenderer.material.SetFloat("_DistortAmoun", val); }
    }

    #endregion


    public void TakeDamage(int damage)
    {
        Health = Mathf.Max(0, Health - damage);
        healthBar.value = Health / (float)maxHealth;

        if (IsBlocking)
        {
            FMODUnity.RuntimeManager.PlayOneShot("event:/FailedAttack");

            return;
        }

        FMODUnity.RuntimeManager.PlayOneShot(hurtEventPath, gameObject.transform.position);
        transform.DOShakePosition(.3f, .5f, 30);
        PlayHitVFX();

        if (Health <= 0)
            Death();
    }

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

    void PlayHitVFX()
    {
        var sequence = DOTween.Sequence();
        sequence
            .AppendCallback(() => SetHit(true))
            .AppendInterval(.2f)
            .AppendCallback(() => SetHit(false));

        void SetHit(bool state) { spriteRenderer.material.SetFloat("_HitEffectBlend", state ? 1f : 0f); }
    }
}
