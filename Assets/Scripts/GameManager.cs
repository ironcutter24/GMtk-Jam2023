using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utility.Patterns;
using UnityEngine.SceneManagement;

public class GameManager : Singleton<GameManager>
{
    [SerializeField]
    private Monster currentOpponent;
    public Monster CurrentOpponent => currentOpponent;

    [SerializeField]
    private HeroData hero = new HeroData();
    public HeroData Hero => hero;

    void Start()
    {
        DontDestroyOnLoad(gameObject);
    }

    public void LoadBattleWith(Monster opponent)
    {
        currentOpponent = opponent;
        SceneManager.LoadScene("CombatScene", LoadSceneMode.Single);
    }

    public void LoadWorldMap()
    {
        SceneManager.LoadScene("WorldScene", LoadSceneMode.Single);
    }

    [System.Serializable]
    public class HeroData
    {
        [SerializeField]
        private Vector2 mapPosition;
        public Vector2 MapPosition => mapPosition;

        [SerializeField]
        private int MaxHealth = 10;

        [SerializeField]
        private int health;
        public int Health => health;

        public HeroData()
        {
            health = MaxHealth;
        }

        public void SetHealth(int val)
        {
            health = val;
        }

        public void SetMapPosition(Vector2 pos)
        {
            mapPosition = pos;
        }
    }
}
