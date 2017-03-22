using System.Collections.Generic;
using UnityEngine;

public class Sheriff : Agent<Sheriff>
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

    // Messaging Events
    public delegate void EncounterOutlaw();
    public static event EncounterOutlaw OnEncounterOutlaw;

    public delegate void SheriffDead(AgentTypes type);
    public static event SheriffDead OnSheriffDead;

    public delegate void SheriffShoots(int damage);
    public static event SheriffShoots OnSheriffShoots;

    // Functions
    /* 
     * public StateMachine<Sheriff> GetFSM()
     * public void Awake()
     * public void Start()
     * public void RespondToSenseEvent(Signal signal)
     *
     * public void DespawnSheriff(AgentTypes type)
     * public void RespawnSheriff(AgentTypes type)
     * public void ReturnGold()
     * public void SearchOutlaw()
     * public void ShootOutlaw()
     * public void ShotByOutlaw()
     * public void YellAtOutlaw()
     */

    public StateMachine<Sheriff> GetFSM()
    {
        return stateMachine;
    }

    public void Awake()
    {
        agentName = "Wyatt";

        isAlive = true;

        controller = GameObject.Find("Controller");
        tilingSystem = controller.GetComponent<TilingSystem>();
        senseManager = controller.GetComponent<SenseManager>();

        stateMachine = new StateMachine<Sheriff>();
        stateMachine.Init(this, CheckLocation.Instance, SheriffGlobalState.Instance);

        // Set up the sprites
        corpseTexture = Resources.Load<Texture2D>("sheriffcorpse");
        texture = Resources.Load<Texture2D>("sheriffsprite");
        corpseSprite = Sprite.Create(corpseTexture, new Rect(0, 0, corpseTexture.width, corpseTexture.height), new Vector2(0.5f, 0.5f));
        sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));

        // Subscribe to the outlaw's messages
        Outlaw.OnOutlawDead += SearchOutlaw;
        Outlaw.OnOutlawShoots += TakeDamage;

        // Subscribe to the undertakers messages
        Undertaker.OnBuriedBody += RespawnSheriff;
        Undertaker.OnCollectedBody += DespawnSheriff;
    }

    public void Start()
    {
        GameObject self = GameObject.Find(agentName);
        if (self != null)
        {
            senseManager.sensors.Add(agentName, new Sensor(AgentTypes.Sheriff, self, modalities, thresholds));
            SenseManager.NotifySheriff += RespondToSenseEvent;
        }

        ChangeLocation(Tiles.SheriffsOffice);
    }

    public void RespondToSenseEvent(Signal signal)
    {
        if(signal.senseEvent == SenseEvents.BankRobbery && isAlive)
        {
            Log("That's the bank alarm!");

            if (currentPath != null)
                ClearCurrentPath();

            FindPath(Tiles.Bank);
            nextState = CheckLocation.Instance;
            ChangeState(Movement<Sheriff>.Instance);
        }
        else
            Log("Oh no, a sense event!");
    }

    public void DespawnSheriff(AgentTypes type)
    {
        if (type == AgentTypes.Sheriff)
        {
            GameObject sheriffObject = GameObject.Find(agentName);
            SpriteRenderer sheriffRenderer = sheriffObject.GetComponent<SpriteRenderer>();
            sheriffRenderer.enabled = false;
        }
    }

    public void RespawnSheriff(AgentTypes type)
    {
        if (type == AgentTypes.Sheriff)
        {
            GameObject sheriffObject = GameObject.Find(agentName);
            SpriteRenderer sheriffRenderer = sheriffObject.GetComponent<SpriteRenderer>();
            sheriffRenderer.sprite = sprite;
            sheriffRenderer.enabled = true;

            isAlive = true;
            health = 5;

            ChangeLocation(Tiles.SheriffsOffice);
            ChangeState(CheckLocation.Instance);
        }
        else
        {
            Log("I hear there's a new Outlaw in town!");

            if (currentPath != null)
                ClearCurrentPath();

            FindPath(destination);
            nextState = CheckLocation.Instance;
            ChangeState(Movement<Sheriff>.Instance);
        }
    }

    public void ReturnGold()
    {
        // The Bank decide not to tell Bob his money has been returned, so just set goldCarried to zero
        goldCarried = 0;
    }

    public void SearchOutlaw(AgentTypes agentType)
    {
        GameObject outlawObject = GameObject.Find(Outlaw.agentName);
        Outlaw outlaw = outlawObject.GetComponent<Outlaw>();
        goldCarried = outlaw.GetGoldCarried();
        outlaw.SetGoldCarried(0);

        Log("Best get this money back to the bank!");
        FindPath(Tiles.Bank);
        nextState = ReturnMoneyToBank.Instance;
        ChangeState(Movement<Sheriff>.Instance);
    }

    public void ShootOutlaw()
    {
        int damage = Random.Range(0, 3);
        if (OnSheriffShoots != null)
            OnSheriffShoots(damage);
    }

    public void ShotByOutlaw()
    {
        Log("I just got shot by the Outlaw!");
        isAlive = false;

        GameObject sheriffObject = GameObject.Find(agentName);
        SpriteRenderer sheriffRenderer = sheriffObject.GetComponent<SpriteRenderer>();
        sheriffRenderer.sprite = corpseSprite;

        if (OnSheriffDead != null)
            OnSheriffDead(AgentTypes.Sheriff);

        ChangeState(Dead<Sheriff>.Instance);
    }

    public void TakeDamage(int damage)
    {
        health -= damage;
        if (health <= 0)
            ShotByOutlaw();
    }

    public void YellAtOutlaw()
    {
        if (OnEncounterOutlaw != null)
            OnEncounterOutlaw();
    }

    public int GetGoldCarried()
    {
        return goldCarried;
    }

    public void SetGoldCarried(int value)
    {
        goldCarried = value;
    }
}
