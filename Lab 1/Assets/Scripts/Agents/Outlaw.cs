using System.Collections.Generic;
using UnityEngine;

public class Outlaw : Agent<Outlaw>
{
    // Variables
    private GameObject controller;
    private int goldCarried = 0;
    private int health = 5;
    private List<double> thresholds = new List<double> { 10.0, 10.0, 10.0 };
    private List<SenseTypes> modalities = new List<SenseTypes> { SenseTypes.Sight, SenseTypes.Hearing, SenseTypes.Smell };
    private Sprite corpseSprite;
    private Sprite sprite;
    private Texture2D corpseTexture;
    private Texture2D texture;

    // Messaging events
    public delegate void BankRobbery(int amount);
    public static event BankRobbery OnBankRobbery;

    public delegate void OutlawDead(AgentTypes type);
    public static event OutlawDead OnOutlawDead;

    public delegate void OutlawShoots(int damage);
    public static event OutlawShoots OnOutlawShoots;

    // Functions
    /* 
     * public StateMachine<Outlaw> GetFSM()
     * public void Awake()
     * public void Start()
     * public void RespondToSenseEvent(Signal signal)
     *
     * public void DespawnOutlaw(AgentTypes type)
     * public void RespawnOutlaw(AgentTypes type)
     * public void RobBank()
     * public void ShotBySheriff()
     * public void StartFight()
     * public void TakeDamage(int damage)
     *
     * public int GetGoldCarried()
     * public void AddToGoldCarried(int amount)
     * public void SetGoldCarried(int value)
     */

    public StateMachine<Outlaw> GetFSM()
    {
        return stateMachine;
    }

    public void Awake()
    {
        agentName = "Jesse";

        isAlive = true;

        controller = GameObject.Find("Controller");
        tilingSystem = controller.GetComponent<TilingSystem>();
        senseManager = controller.GetComponent<SenseManager>();

        stateMachine = new StateMachine<Outlaw>();
        stateMachine.Init(this, LurkInCamp.Instance, OutlawGlobalState.Instance);

        // Set up the sprites
        corpseTexture = Resources.Load<Texture2D>("outlawcorpse");
        texture = Resources.Load<Texture2D>("outlaw");
        corpseSprite = Sprite.Create(corpseTexture, new Rect(0, 0, corpseTexture.width, corpseTexture.height), new Vector2(0.5f, 0.5f));
        sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));

        // Subscribe to the sheriff's messages
        Sheriff.OnEncounterOutlaw += StartFight;
        Sheriff.OnSheriffDead += SearchSheriff;
        Sheriff.OnSheriffShoots += TakeDamage;

        // Subscribe to the undertaker's messages
        Undertaker.OnBuriedBody += RespawnOutlaw;
        Undertaker.OnCollectedBody += DespawnOutlaw;
    }

    public void Start()
    {
        GameObject self = GameObject.Find(agentName);
        if (self != null)
        {
            senseManager.sensors.Add(agentName, new Sensor(AgentTypes.Outlaw, self, modalities, thresholds));
            SenseManager.NotifyOutlaw += RespondToSenseEvent;
        }

        ChangeLocation(Tiles.OutlawCamp);
    }

    public void RespondToSenseEvent(Signal signal)
    {
        Log("Oh no, a sense event!");
    }

    public void DespawnOutlaw(AgentTypes type)
    {
        if (type == AgentTypes.Outlaw)
        {
            GameObject outlawObject = GameObject.Find(agentName);
            SpriteRenderer outlawRenderer = outlawObject.GetComponent<SpriteRenderer>();
            outlawRenderer.enabled = false;
        }
    }

    public void RespawnOutlaw(AgentTypes type)
    {
        if (type == AgentTypes.Outlaw)
        {
            GameObject outlawObject = GameObject.Find(agentName);
            SpriteRenderer outlawRenderer = outlawObject.GetComponent<SpriteRenderer>();
            outlawRenderer.sprite = sprite;
            outlawRenderer.enabled = true;

            isAlive = true;
            health = 5;

            ChangeLocation(Tiles.OutlawCamp);
            ChangeState(LurkInCamp.Instance);
        }
        else
        {
            Log("I hear there's a new Sheriff in town!");
        }
    }

    public void RobBank()
    {
        int stolen_amount = Random.Range(1, 10);
        goldCarried += stolen_amount;
        if (OnBankRobbery != null)
            OnBankRobbery(stolen_amount);
        senseManager.AddSignal(new Signal(40, new Hearing(), SenseEvents.BankRobbery, currentLocation));
    }

    public void SearchSheriff(AgentTypes agentType)
    {
        GameObject sheriffObject = GameObject.Find(Sheriff.agentName);
        Sheriff sheriff = sheriffObject.GetComponent<Sheriff>();
        goldCarried = sheriff.GetGoldCarried();
        sheriff.SetGoldCarried(0);

        Log("Gonna go lurk in my camp again!");
        FindPath(Tiles.OutlawCamp);
        nextState = LurkInCamp.Instance;
        ChangeState(Movement<Outlaw>.Instance);
    }

    public void ShootSheriff()
    {
        int damage = Random.Range(0, 3);
        if (OnOutlawShoots != null)
            OnOutlawShoots(damage);
    }

    public void ShotBySheriff()
    {
        Log("I just got shot by the Sheriff!");
        isAlive = false;

        GameObject outlawObject = GameObject.Find(agentName);
        SpriteRenderer outlawRenderer = outlawObject.GetComponent<SpriteRenderer>();
        outlawRenderer.sprite = corpseSprite;

        if (OnOutlawDead != null)
            OnOutlawDead(AgentTypes.Outlaw);

        ChangeState(Dead<Outlaw>.Instance);
    }

    public void StartFight()
    {
        if (currentPath != null)
            ClearCurrentPath();
        ChangeState(FightSheriff.Instance);
    }

    public void TakeDamage(int damage)
    {
        health -= damage;
        if (health <= 0)
            ShotBySheriff();
    }

    public int GetGoldCarried()
    {
        return goldCarried;
    }

    public void AddToGoldCarried(int amount)
    {
        goldCarried += amount;
        if (goldCarried < 0)
            goldCarried = 0;
    }

    public void SetGoldCarried(int value)
    {
        goldCarried = value;
    }
}
