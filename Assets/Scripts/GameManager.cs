using System.Collections;
using System.Collections.Generic;
using FMODUnity;
using UnityEngine;
using Utility.Patterns;
using UnityEngine.SceneManagement;

public class GameManager : Singleton<GameManager>
{
    [SerializeField]
    private Monster currentOpponent;
    public Monster CurrentOpponent => currentOpponent;
    public MusicManager musicManager;

    [SerializeField]
    private HeroData hero = new HeroData();
    public HeroData Hero => hero;


    private void Start()
    {
        DontDestroyOnLoad(gameObject);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }

        //Debug.Log("Hero health: " + hero.Health);
    }

    public void LoadBattleWith(Monster opponent)
    {
        currentOpponent = opponent;
        MusicManager.Instance.musicEvent.setParameterByNameWithLabel("CurrentScreen", "Combat");
        SceneManager.LoadScene("CombatScene", LoadSceneMode.Single);
    }

    public void LoadWorldMap()
    {
        MusicManager.Instance.musicEvent.setParameterByNameWithLabel("CurrentScreen", "Map");
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

        [SerializeField]
        private int attackDamage;
        public int AttackDamage => attackDamage;

        public HeroData()
        {
            health = MaxHealth;
        }

        public void SetHealth(int val)
        {
            health = Mathf.Clamp(val, 0, MaxHealth);
        }

        public void RestoreHealth(int val)
        {
            health = Mathf.Clamp(health + val, 0, MaxHealth);
            FMODUnity.RuntimeManager.PlayOneShot("event:/HealthPickup");
        }

        public void IncreaseAttackDamage(int amount)
        {
            attackDamage += amount;
            FMODUnity.RuntimeManager.PlayOneShot("event:/PowerUp");
        }

        public void SetMapPosition(Vector2 pos)
        {
            mapPosition = pos;
        }
    }
}
