using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroPawn : MonoBehaviour
{
    [SerializeField]
    float moveSpeed = 2f;

    bool isMoving = false;

    public void TryMove()
    {
        if (isMoving) return;

        isMoving = true;
        transform.DOMove(GetNextLocation().Pos, moveSpeed)
            .SetSpeedBased()
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
