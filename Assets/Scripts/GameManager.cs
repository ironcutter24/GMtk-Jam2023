using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utility.Patterns;

public class GameManager : Singleton<GameManager>
{
    [SerializeField]
    private Vector2 heroMapPosition = Vector2.zero;
    public Vector2 HeroMapPosition => heroMapPosition;

    void Start()
    {
        DontDestroyOnLoad(gameObject);
    }

    public void StoreHeroPosition(Vector2 pos)
    {
        heroMapPosition = pos;
    }
}
