using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HeroPawn : MonoBehaviour
{
    [SerializeField]
    SpriteRenderer graphics;

    [SerializeField]
    float moveSpeed = 2f;

    bool isMoving = false;
    public GameObject beginPanel;

    private void Start()
    {
        transform.position = GameManager.Instance.Hero.MapPosition;
        StartCoroutine(_TestPath());
        beginPanel.SetActive(false);
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
                beginPanel.SetActive(true);
                while (true)
                {
                    yield return null;
                    if (Input.anyKeyDown)
                        break;
                }

                GameManager.Instance.Hero.SetMapPosition(transform.position);
                combatLocation.InitBattle();
                yield break;
            }

            var powerupLocation = GetCurrentLocation().GetComponent<PowerupLocation>();
            if (powerupLocation)
            {
                powerupLocation.UpdateHeroStatus();
                powerupLocation.DisplayPanel();
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
        if (targetLocations.Count > 0)
        {
            return targetLocations[Random.Range(0, targetLocations.Count)];
        }
        else {
            return null;
        }
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
