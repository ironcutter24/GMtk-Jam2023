using System.Collections;
using System.Collections.Generic;
using FMODUnity;
using UnityEngine;
using Utility.Patterns;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class GameManager : Singleton<GameManager>
{
    [SerializeField]
    private Monster currentOpponent;
    public Monster CurrentOpponent => currentOpponent;
    public MusicManager musicManager;

    [SerializeField]
    private HeroData hero = new HeroData();
    public HeroData Hero => hero;

    public bool hasBegun = false;


    private void Start()
    {
        DontDestroyOnLoad(gameObject);
        hasBegun = false;
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

    public void RestartGame()
    {
        hasBegun = false;
        //reset hero position
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

        public Slider healthSlider;
        public TextMeshProUGUI healthLabel;

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
            healthSlider.value = Health;
            healthLabel.SetText("HP: " + Health.ToString());
        }

        public void RestoreHealth(int val)
        {
            SetHealth(val);
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
