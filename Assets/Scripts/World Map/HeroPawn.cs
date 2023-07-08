using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroPawn : MonoBehaviour
{
    [SerializeField]
    SpriteRenderer graphics;

    [SerializeField]
    float moveSpeed = 2f;

    bool isMoving = false;

    private void Start()
    {
        transform.position = GameManager.Instance.Hero.MapPosition;
        StartCoroutine(_TestPath());
    }

    IEnumerator _TestPath()
    {
        while (true)
        {
            yield return new WaitForSeconds(1f);
            TryMove();
            yield return new WaitUntil(() => !isMoving);

            var combatLocation = GetCurrentLocation().GetComponent<CombatLocation>();
            if (combatLocation)
            {
                PlayReadyAnimation();

                while (true)
                {
                    yield return null;
                    if (Input.GetKeyDown(KeyCode.Space))
                        break;
                }

                GameManager.Instance.Hero.SetMapPosition(transform.position);
                combatLocation.InitBattle();
                yield break;
            }
        }
    }

    void PlayReadyAnimation()
    {
        graphics.transform.DOMoveY(.6f, .3f)
            .SetLoops(-1, LoopType.Yoyo)
            .SetRelative();
    }

    public void TryMove()
    {
        if (isMoving) return;

        isMoving = true;
        transform.DOMove(GetNextLocation().Pos, moveSpeed)
            .SetSpeedBased()
            .SetEase(Ease.Linear)
            .OnComplete(() => isMoving = false);
    }

    private WorldLocation GetNextLocation()
    {
        var targetLocations = GetCurrentLocation().Children;
        return targetLocations[Random.Range(0, targetLocations.Count)];
    }

    private WorldLocation GetCurrentLocation()
    {
        var hits = Physics2D.OverlapCircleAll(transform.position, .1f);

        foreach (var hit in hits)
        {
            var comp = hit.gameObject.GetComponent<WorldLocation>();
            if (comp) return comp;
        }
        throw new System.Exception("Current world location not found!");
    }
}
