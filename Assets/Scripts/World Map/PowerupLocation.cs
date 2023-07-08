using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PowerupLocation : MonoBehaviour
{
    public abstract void UpdateHeroStatus();

    protected abstract void OnDrawGizmos();
}
