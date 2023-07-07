using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldLocation : MonoBehaviour
{
    [SerializeField]
    List<WorldLocation> children;

    public List<WorldLocation> Children;

    public Vector3 Pos => transform.position;
}
