using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldLocation : MonoBehaviour
{
    [SerializeField]
    List<WorldLocation> children = new List<WorldLocation>();

    public List<WorldLocation> Children => children;

    public Vector3 Pos => transform.position;

    private void OnDrawGizmos()
    {
        if (children.Count > 0)
        {
            Gizmos.color = Color.white;
            foreach (var child in children)
            {
                if (child != null)
                    Gizmos.DrawLine(transform.position, child.transform.position);
            }
        }
    }
}
