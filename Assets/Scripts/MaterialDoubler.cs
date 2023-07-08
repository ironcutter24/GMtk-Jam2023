using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaterialDoubler : MonoBehaviour
{
    private void Awake()
    {
        var renderer = GetComponent<SpriteRenderer>();
        renderer.material = Material.Instantiate(renderer.material);
    }
}
