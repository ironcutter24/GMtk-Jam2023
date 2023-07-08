using DG.Tweening;
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

    //[SerializeField]
    //protected Slider cooldownBar;

    [SerializeField]
    protected int attackDamage = 1;


    [FMODUnity.EventRef]
    public string attackEventPath;

    [FMODUnity.EventRef]
    public string hurtEventPath;

    [FMODUnity.EventRef]
    public string specialEventPath;

    //[SerializeField]
    //protected float attackCoolDown = 3f;
    //protected bool onCoolDown = false;
    //public float coolDownRemaining;

    protected const int maxHealth = 10;
    protected int Health { get; private set; }



    protected Vector3 startPos { get; private set; }
    public bool IsActing { get; protected set; }

    public bool IsDead => Health <= 0;


    protected virtual void Awake()
    {
        Health = maxHealth;
        startPos = transform.position;
    }

    public void SetManager(EncounterManager encounterManager)
    {
        this.encounterManager = encounterManager;
    }

    public void SimpleAttack()
    {
        if (IsActing) return;

        IsActing = true;

        Vector3 targetPos = Opponent.transform.position;
        MoveToEnemy(startPos, targetPos, .1f,
            () => Opponent.TakeDamage(attackDamage),
            () => IsActing = false);
        FMODUnity.RuntimeManager.PlayOneShot(attackEventPath, gameObject.transform.position);
    }

    public void TakeDamage(int damage
    {
        Health = Mathf.Max(0, Health - damage);
        healthBar.value = Health / (float)maxHealth;
        FMODUnity.RuntimeManager.PlayOneShot(hurtEventPath, gameObject.transform.position);

        if (Health <= 0)
        {
            // Death
        }

    }

    public void SpecialAttack() {
        FMODUnity.RuntimeManager.PlayOneShot(specialEventPath, gameObject.transform.position);
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
