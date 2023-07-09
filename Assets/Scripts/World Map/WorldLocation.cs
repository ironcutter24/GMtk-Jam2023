using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldLocation : MonoBehaviour
{
    [SerializeField]
    GameObject pathSegmentPrefab;

    [SerializeField]
    WorldLocation diversionChild;

    [SerializeField]
    List<WorldLocation> children = new List<WorldLocation>();

    public WorldLocation Parent { get; private set; }
    public List<WorldLocation> Children => diversionChild ? new List<WorldLocation> { diversionChild } : children;

    public Vector3 Pos => transform.position;

    private void Start()
    {
        foreach (var child in children)
        {
            var path = Instantiate(pathSegmentPrefab, transform);

            var toChild = child.transform.position - transform.position;
            path.transform.localPosition = toChild / 2f;

            path.transform.rotation = Quaternion.Euler(0f, 0f, -Vector2.SignedAngle(toChild, Vector2.right));

            var targetScale = path.transform.localScale;
            targetScale.x = toChild.magnitude;
            path.transform.localScale = targetScale;
        }
    }

    public void InvalidateDiversion()
    {
        diversionChild = null;
    }

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
