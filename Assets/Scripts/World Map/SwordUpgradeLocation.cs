using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordUpgradeLocation : PowerupLocation
{
    [SerializeField]
    private int upgradeAmount = 1;

    public override void UpdateHeroStatus()
    {
        GameManager.Instance.Hero.IncreaseAttackDamage(upgradeAmount);
    }

    protected override void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawCube(transform.position, Vector3.one * .4f);
    }
}
