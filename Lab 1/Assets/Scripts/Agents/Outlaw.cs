using System.Collections.Generic;
using UnityEngine;

public class Outlaw : Agent<Outlaw>
{
    // Variables
    private GameObject controller;
    private int goldCarried = 0;
    private List<double> thresholds = new List<double> { 10.0, 10.0, 10.0 };
    private List<SenseTypes> modalities = new List<SenseTypes> { SenseTypes.Sight, SenseTypes.Hearing, SenseTypes.Smell };
    public static string agentName = "Jesse";

    // Messaging events
    public delegate void BankRobbery(int amount);
    public static event BankRobbery OnBankRobbery;

    public delegate void OutlawDead(AgentTypes type);
    public static event OutlawDead OnOutlawDead;

    // Functions
    /* 
     * public StateMachine<Outlaw> GetFSM()
     * public void Awake()
     * public void ChangeLocation(Tiles location)
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
        isAlive = true;

        controller = GameObject.Find("Controller");
        tilingSystem = controller.GetComponent<TilingSystem>();
        senseManager = controller.GetComponent<SenseManager>();

        stateMachine = new StateMachine<Outlaw>();
        stateMachine.Init(this, LurkInCamp.Instance, OutlawGlobalState.Instance);

        // Subscribe to the sheriff's messages
        Sheriff.OnEncounterOutlaw += StartFight;
        Sheriff.OnSheriffShoots += TakeDamage;

        // Subscribe to the undertaker's messages
        Undertaker.OnBuriedBody += RespawnOutlaw;
        Undertaker.OnCollectedBody += DespawnOutlaw;
    }

    public void ChangeLocation(Tiles location)
    {
        this.location = location;
        currentLocation = tilingSystem.locations[(int)location];
        transform.position = new Vector3(currentLocation.x - tilingSystem.CurrentPosition.x, currentLocation.y - tilingSystem.CurrentPosition.y, 0);
    }

    public void Start()
    {
        GameObject self = GameObject.Find(agentName);
        if (self != null)
        {
            senseManager.sensors.Add(new Sensor(AgentTypes.Outlaw, self, modalities, thresholds, agentName));
            SenseManager.NotifyOutlaw += RespondToSenseEvent;
        }

        ChangeLocation(Tiles.OutlawCamp);
    }

    public void RespondToSenseEvent(Signal signal)
    {
        Debug.LogError("Outlaw: Oh no, a sense event!");
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
            outlawRenderer.enabled = true;
            Debug.Log("Outlaw: Renderer enabled again!");

            isAlive = true;

            ChangeLocation(Tiles.OutlawCamp);
            ChangeState(LurkInCamp.Instance);
        }
        else
        {
            Debug.Log("Outlaw: I hear there's a new Sheriff in town!");
        }
    }

    public void RobBank()
    {
        int stolen_amount = Random.Range(1, 10);
        goldCarried += stolen_amount;
        if (OnBankRobbery != null)
            OnBankRobbery(stolen_amount);
    }

    public void ShotBySheriff()
    {
        Debug.Log("Outlaw: I just got shot by the Sheriff!");
        isAlive = false;

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
        if (damage >= 0)
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
