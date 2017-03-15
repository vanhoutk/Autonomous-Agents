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
    private List<double> thresholds = new List<double> { 10.0, 10.0, 10.0 };
    private List<SenseTypes> modalities = new List<SenseTypes> { SenseTypes.Sight, SenseTypes.Hearing, SenseTypes.Smell };
    public static string agentName = "Miner";

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
        controller = GameObject.Find("Controller");
        tilingSystem = controller.GetComponent<TilingSystem>();
        senseManager = controller.GetComponent<SenseManager>();

        stateMachine = new StateMachine<Miner>();
        stateMachine.Init(this, GoHomeAndSleepTilRested.Instance);
        Outlaw.OnBankRobbery += RespondToBankRobbery;
    }

    public void Start()
    {
        GameObject self = GameObject.Find(agentName);
        if (self != null)
        {
            senseManager.sensors.Add(new Sensor(AgentTypes.Miner, self, modalities, thresholds, agentName));
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
        Debug.Log("Miner: My Money!");
    }

    public void RespondToSenseEvent(Signal signal)
    {
        Debug.Log("Miner: Oh no, a sense event!");
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
    }

    public void DecreaseFatigue()
    {
        fatigue = 0;
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
