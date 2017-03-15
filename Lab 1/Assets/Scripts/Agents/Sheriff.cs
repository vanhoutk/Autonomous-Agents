using System.Collections.Generic;
using UnityEngine;

public class Sheriff : Agent<Sheriff>
{
    // Variables
    private GameObject controller;
    private List<double> thresholds = new List<double> { 10.0, 10.0, 10.0 };
    private List<SenseTypes> modalities = new List<SenseTypes> { SenseTypes.Sight, SenseTypes.Hearing, SenseTypes.Smell };
    public static string agentName = "Wyatt";

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
        isAlive = true;

        controller = GameObject.Find("Controller");
        tilingSystem = controller.GetComponent<TilingSystem>();
        senseManager = controller.GetComponent<SenseManager>();

        stateMachine = new StateMachine<Sheriff>();
        stateMachine.Init(this, CheckLocation.Instance, SheriffGlobalState.Instance);

        // Subscribe to the undertakers messages
        Undertaker.OnBuriedBody += RespawnSheriff;
        Undertaker.OnCollectedBody += DespawnSheriff;
    }

    public void Start()
    {
        GameObject self = GameObject.Find(agentName);
        if (self != null)
        {
            senseManager.sensors.Add(new Sensor(AgentTypes.Sheriff, self, modalities, thresholds, agentName));
            SenseManager.NotifySheriff += RespondToSenseEvent;
        }

        ChangeLocation(Tiles.SheriffsOffice);
    }

    public void RespondToSenseEvent(Signal signal)
    {
        Debug.Log("Sheriff: Oh no, a sense event!");
    }

    public void DespawnSheriff(AgentTypes type)
    {
        if (type == AgentTypes.Sheriff)
        {
            GameObject sheriffObject = GameObject.Find(Outlaw.agentName);
            SpriteRenderer sheriffRenderer = sheriffObject.GetComponent<SpriteRenderer>();
            sheriffRenderer.enabled = false;
        }
    }

    public void RespawnSheriff(AgentTypes type)
    {
        if (type == AgentTypes.Sheriff)
        {
            GameObject sheriffObject = GameObject.Find(Outlaw.agentName);
            SpriteRenderer sheriffRenderer = sheriffObject.GetComponent<SpriteRenderer>();
            sheriffRenderer.enabled = true;

            isAlive = true;

            ChangeLocation(Tiles.SheriffsOffice);
            ChangeState(WaitInSheriffOffice.Instance);
        }
        else
        {
            Debug.Log("Sheriff: I hear there's a new Outlaw in town!");

            if(stateMachine.GetState() != CheckLocation.Instance)
                ChangeState(CheckLocation.Instance);
        }
    }

    public void ShootOutlaw()
    {
        if (OnSheriffShoots != null)
            OnSheriffShoots(1);
    }

    public void ShotByOutlaw()
    {
        isAlive = false;
        if (OnSheriffDead != null)
            OnSheriffDead(AgentTypes.Sheriff);
    }

    public void YellAtOutlaw()
    {
        if (OnEncounterOutlaw != null)
            OnEncounterOutlaw();
    }
}
