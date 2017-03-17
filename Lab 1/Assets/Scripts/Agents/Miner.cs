using System.Collections.Generic;
using UnityEngine;

public class Miner : Agent<Miner>
{
    // Variables
    private const int maxGold = 3;
    private const int maxThirst = 50;
    private const int maxFatigue = 5;
    private const int comfortLevel = 5;

    private int goldCarried = 0;
    private int moneyInBank = 0;
    private int thirst = 0;
    private int fatigue = 0;

    private GameObject controller;
    private GameObject self;
    private List<double> thresholds = new List<double> { 10.0, 10.0, 10.0 };
    private List<SenseTypes> modalities = new List<SenseTypes> { SenseTypes.Sight, SenseTypes.Hearing, SenseTypes.Smell };

    // Functions
    /*
     * public bool Fatigued()
     * public bool PocketsFull()
     * public bool Thirsty()
     * public bool WealthyEnough()
     *
     * public int GetGoldCarried()
     * public int GetMoneyInBank()
     *
     * public StateMachine<Miner> GetFSM()
     * public void Awake()
     * public void Start()
     * public void RespondToBankRobbery()
     * public void RespondToSenseEvent(Signal signal)
     *
     * public void AddToBank(int amount)
     * public void AddToGoldCarried(int amount)
     * public void BuyAndDrinkWhiskey()
     * public void DecreaseFatigue()
     * public void IncreaseFatigue()
     * public void SetGoldCarried(int value)
     */

    public bool Fatigued()
    {
        return (fatigue >= maxFatigue);
    }

    public bool PocketsFull()
    {
        return (goldCarried >= maxGold);
    }

    public bool Thirsty()
    {
        return (thirst >= maxThirst);
    }

    public bool WealthyEnough()
    {
        return (moneyInBank >= comfortLevel);
    }

    public int GetGoldCarried()
    {
        return goldCarried;
    }

    public int GetMoneyInBank()
    {
        return moneyInBank;
    }

    public override void Update()
    {
        thirst++;
        stateMachine.Update();
    }

    public StateMachine<Miner> GetFSM()
    {
        return stateMachine;
    }

    public void Awake()
    {
        agentName = "Bob";

        controller = GameObject.Find("Controller");
        tilingSystem = controller.GetComponent<TilingSystem>();
        senseManager = controller.GetComponent<SenseManager>();

        stateMachine = new StateMachine<Miner>();
        stateMachine.Init(this, GoHomeAndSleepTilRested.Instance);
        Outlaw.OnBankRobbery += RespondToBankRobbery;
    }

    public void Start()
    {
        self = GameObject.Find(agentName);
        if (self != null)
        {
            senseManager.sensors.Add(agentName, new Sensor(AgentTypes.Miner, self, modalities, thresholds));
            SenseManager.NotifyMiner += RespondToSenseEvent;
        }

        ChangeLocation(Tiles.Shack);
    }

    public void RespondToBankRobbery(int amount)
    {
        if (amount > moneyInBank)
            moneyInBank = 0;
        else
            moneyInBank -= amount;
        Log("That Outlaw stole my Money!");
    }

    public void RespondToSenseEvent(Signal signal)
    {
        Log("Oh no, a sense event!");
    }

    public void AddToBank(int amount)
    {
        moneyInBank += amount;
    }

    public void AddToGoldCarried(int amount)
    {
        goldCarried += amount;
        if (goldCarried < 0)
            goldCarried = 0;
    }

    public void BuyAndDrinkWhiskey()
    {
        thirst = 0;
        moneyInBank -= 2;

        // Update sensory perceptiveness values
        if (self != null)
        {
            senseManager.sensors.Remove(agentName);
            thresholds[0] = 5.0; // Reduce ability to see and hear
            thresholds[1] = 5.0;
            senseManager.sensors.Add(agentName, new Sensor(AgentTypes.Miner, self, modalities, thresholds));
        }
            
    }

    public void DecreaseFatigue()
    {
        fatigue = 0;

        // Return sensory perceptiveness to normal
        if (self != null)
        {
            senseManager.sensors.Remove(agentName);
            thresholds[0] = 10.0; // Reduce ability to see and hear
            thresholds[1] = 10.0;
            senseManager.sensors.Add(agentName, new Sensor(AgentTypes.Miner, self, modalities, thresholds));
        }
    }

    public void IncreaseFatigue()
    {
        fatigue++;
    }

    public void SetGoldCarried(int value)
    {
        goldCarried = value;
    }
}
