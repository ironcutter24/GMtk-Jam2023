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
    public GameObject beginPanel;

    [SerializeField]
    private IntroPanel introPanel;

    [SerializeField]
    private GameObject gameOverPanel;

    private void Start()
    {
        transform.position = GameManager.Instance.Hero.MapPosition;
        StartCoroutine(_ProgressPath());
        beginPanel.SetActive(false);
        gameOverPanel.SetActive(false);
    }

    IEnumerator _ProgressPath()
    {
        while (true)
        {
            yield return new WaitForSeconds(1f);

            if (!GameManager.Instance.hasBegun)
            {
                yield return introPanel._ShowIntro();
                yield return new WaitForSeconds(1f);
            }

            var currentLocation = GetCurrentLocation();
            if (currentLocation.Children.Count <= 0)
            {
                yield return _GameOver();
                yield break;
            }

            TryMoveFrom(currentLocation);
            yield return new WaitUntil(() => !isMoving);

            var combatLocation = GetCurrentLocation().GetComponent<CombatLocation>();
            if (combatLocation)
            {
                yield return _ReadyForCombat();
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

    IEnumerator _ReadyForCombat()
    {
        PlayReadyAnimation();
        beginPanel.SetActive(true);

        yield return new WaitUntil(() => Input.anyKeyDown);

        GameManager.Instance.Hero.SetMapPosition(transform.position);
    }

    IEnumerator _GameOver()
    {
        Debug.Log("Game Over");
        gameOverPanel.SetActive(true);

        yield return new WaitUntil(() => Input.anyKeyDown);

        GameManager.Instance.RestartGame();
    }

    void PlayReadyAnimation()
    {
        graphics.transform.DOMoveY(.6f, .3f)
            .SetLoops(-1, LoopType.Yoyo)
            .SetRelative();
    }

    public void TryMoveFrom(WorldLocation currentLocation)
    {
        if (isMoving) return;
        isMoving = true;

        var nextLocation = GetNextLocation(currentLocation.Children);
        transform.DOMove(nextLocation.Pos, moveSpeed)
            .SetSpeedBased()
            .SetEase(Ease.Linear)
            .OnComplete(() => isMoving = false);
    }

    private WorldLocation GetNextLocation(List<WorldLocation> availableLocations)
    {
        return availableLocations[Random.Range(0, availableLocations.Count)];
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
