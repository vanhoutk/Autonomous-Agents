using System.Collections.Generic;
using UnityEngine;

public class Undertaker : Agent<Undertaker>
{
    // Variables
    private AgentTypes bodyType;
    private GameObject controller;
    private List<double> thresholds = new List<double> { 10.0, 10.0, 10.0 };
    private List<SenseTypes> modalities = new List<SenseTypes> { SenseTypes.Sight, SenseTypes.Hearing, SenseTypes.Smell };
    private string agentName = "Under";

    // Message Events
    public delegate void BuriedBody(AgentTypes type);
    public static event BuriedBody OnBuriedBody;

    public delegate void CollectedBody(AgentTypes type);
    public static event CollectedBody OnCollectedBody;

    // Functions
    /*
     * public StateMachine<Undertaker> GetFSM()
     * public void Awake()
     * 
     * public void BuryBody()
     * public void CollectABody()
     * public void RespondToDeath(AgentTypes type)
     */

    public StateMachine<Undertaker> GetFSM()
    {
        return stateMachine;
    }

    public void Awake()
    {
        controller = GameObject.Find("Controller");
        tilingSystem = controller.GetComponent<TilingSystem>();
        senseManager = controller.GetComponent<SenseManager>();

        stateMachine = new StateMachine<Undertaker>();
        stateMachine.Init(this, WaitInUndertakers.Instance);

        // Subscribe to outlaw and sheriff death events
        Outlaw.OnOutlawDead += RespondToDeath;
        Sheriff.OnSheriffDead += RespondToDeath;
    }

    public void Start()
    {
        GameObject self = GameObject.Find(agentName);
        if (self != null)
        {
            senseManager.sensors.Add(new Sensor(AgentTypes.Undertaker, self, modalities, thresholds, agentName));
            SenseManager.NotifyUndertaker += RespondToSenseEvent;
        }

        ChangeLocation(Tiles.Undertakers);
    }

    public void RespondToSenseEvent(Signal signal)
    {
        Debug.Log("Undertaker: Oh no, a sense event!");
    }

    public void BuryBody()
    {
        if(OnBuriedBody != null)
            OnBuriedBody(bodyType);
    }

    public void CollectABody()
    {
        if (OnCollectedBody != null)
            OnCollectedBody(bodyType);
    }

    public void RespondToDeath(AgentTypes type)
    {
        bodyType = type;

        if(type == AgentTypes.Outlaw)
        {
            Debug.Log("Undertaker: The Sheriff has killed the Outlaw!");

            GameObject outlawObject = GameObject.Find(Outlaw.agentName);
            Outlaw outlaw = outlawObject.GetComponent<Outlaw>();

            // Clear the current path if we have one
            if (currentPath != null)
                ClearCurrentPath();

            FindPath(outlaw.currentLocation);
            Debug.Log("Undertaker: Going to pick up the Outlaw's body!");
        }
        else
        {
            Debug.Log("Undertaker: The Outlaw has killed the Sheriff!");
            GameObject sheriffObject = GameObject.Find(Sheriff.agentName);
            Sheriff sheriff = sheriffObject.GetComponent<Sheriff>();

            // Clear the current path if we have one
            if (currentPath != null)
                ClearCurrentPath();

            FindPath(sheriff.currentLocation);
            Debug.Log("Undertaker: Going to pick up the Sheriff's body!");
        }

        nextState = CollectBody.Instance;
        ChangeState(Movement<Undertaker>.Instance);
    }
}
