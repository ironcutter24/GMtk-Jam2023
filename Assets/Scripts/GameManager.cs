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

    public bool hasBegun = false;
    FMOD.Studio.Bus MasterBus;

    protected override void Awake()
    {
        base.Awake();

        DontDestroyOnLoad(gameObject);
        hasBegun = false;

        hero.Reset();
        MasterBus = FMODUnity.RuntimeManager.GetBus("Bus:/");
    }


    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
    }

    public void LoadBattleWith(Monster opponent)
    {
        currentOpponent = opponent;
        MusicManager.Instance.musicEvent.setParameterByNameWithLabel("CurrentScreen", "Combat");
        SceneManager.LoadScene("CombatScene", LoadSceneMode.Single);
    }

    public void LoadWorldMap()
    {
        StartCoroutine(_LoadWorldMap());

        IEnumerator _LoadWorldMap()
        {
            var sceneLoader = SceneManager.LoadSceneAsync("WorldScene", LoadSceneMode.Single);

            yield return new WaitUntil(() => sceneLoader.progress > .98f);

            sceneLoader.allowSceneActivation = true;
            MusicManager.Instance.musicEvent.setParameterByNameWithLabel("CurrentScreen", "Map");
        }
    }

    public void RestartGame()
    {
        hasBegun = false;
        hero.Reset();
        MasterBus.stopAllEvents(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
        SceneManager.LoadScene("WorldScene", LoadSceneMode.Single);
    }

    [System.Serializable]
    public class HeroData
    {
        [SerializeField]
        private Vector2 startingMapPosition = Vector2.zero;
        public Vector2 MapPosition { get; private set; }

        [SerializeField]
        private int maxHealth = 10;
        public int MaxHealth => maxHealth;

        public int Health { get; private set; }
        public float NormalizedHealth => Health / (float)maxHealth;

        [SerializeField]
        private int startingAttackDamage = 1;
        public int AttackDamage { get; private set; }


        public void Reset()
        {
            MapPosition = startingMapPosition;
            Health = maxHealth;
            AttackDamage = startingAttackDamage;
        }

        public void SetHealth(int val)
        {
            Health = Mathf.Clamp(val, 0, maxHealth);
        }

        public void RestoreHealth(int val)
        {
            SetHealth(Health + val);
            FMODUnity.RuntimeManager.PlayOneShot("event:/HealthPickup");
        }

        public void IncreaseAttackDamage(int amount)
        {
            AttackDamage += amount;
            FMODUnity.RuntimeManager.PlayOneShot("event:/PowerUp");
        }

        public void SetMapPosition(Vector2 pos)
        {
            MapPosition = pos;
        }

    }
}
