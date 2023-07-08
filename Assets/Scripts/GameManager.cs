using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utility.Patterns;
using UnityEngine.SceneManagement;

public class GameManager : Singleton<GameManager>
{
    [SerializeField]
    private Vector2 heroMapPosition = Vector2.zero;
    public Vector2 HeroMapPosition => heroMapPosition;

    public Monster CurrentOpponent { get; private set; }

    void Start()
    {
        DontDestroyOnLoad(gameObject);
    }

    public void StoreHeroPosition(Vector2 pos)
    {
        heroMapPosition = pos;
    }

    public void LoadBattleWith(Monster opponent)
    {
        CurrentOpponent = opponent;
        SceneManager.LoadScene("CombatScene", LoadSceneMode.Single);
    }
}
