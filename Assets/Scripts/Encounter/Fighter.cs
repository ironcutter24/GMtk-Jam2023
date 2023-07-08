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

    [SerializeField]
    protected int attackDamage = 1;

    [SerializeField]
    protected int maxHealth = 10;
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
    }

    public void TakeDamage(int damage)
    {
        Health = Mathf.Max(0, Health - damage);
        healthBar.value = Health / (float)maxHealth;

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



    Timer frozenTimer = new Timer();
    public bool IsFrozen => frozenTimer.IsExpired;
    public void ApplyFreeze()
    {
        frozenTimer.Set(1f);
    }


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


    Timer blockignTimer = new Timer();
    public bool IsBlocking { get; private set; } = false;
    public void ApplyBlocking()
    {
        blockignTimer.Set(1f);
    }
}
