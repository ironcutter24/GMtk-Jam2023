using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealingLocation : PowerupLocation
{
    [SerializeField]
    private int healingAmount = 5;

    public override void UpdateHeroStatus()
    {
        GameManager.Instance.Hero.RestoreHealth(healingAmount);
    }

    protected override void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawCube(transform.position, Vector3.one * .4f);
    }
}
