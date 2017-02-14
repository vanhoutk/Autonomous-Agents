using System.Collections;
using UnityEngine;

public class Miner : Agent
{
    private const int maxGold = 3;
    private const int maxThirst = 5;
    private const int maxFatigue = 5;
    private const int comfortLevel = 5;
    private int goldCarried = 0;
    private int moneyInBank = 0;
    private int thirst = 0;
    private int fatigue = 0;

    private GameObject controller;
    private StateMachine<Miner> stateMachine;

    private Tiles location;

    public TilingSystem tilingSystem;
    public Vector2 currentLocation;

    public void Awake()
    {
        this.controller = GameObject.Find("Controller");
        this.tilingSystem = controller.GetComponent<TilingSystem>();

        this.stateMachine = new StateMachine<Miner>();
        this.stateMachine.Init(this, GoHomeAndSleepTilRested.Instance);
        Outlaw.OnBankRobbery += RespondToBankRobbery;
    }

    public void ChangeState(State<Miner> state)
    {
        this.stateMachine.ChangeState(state);
    }

    public override void Update()
    {
        this.thirst++;
        this.stateMachine.Update();
    }

    public Tiles GetLocation()
    {
        return this.location;
    }

    public void ChangeLocation(Tiles location)
    {
        Debug.Log("Miner: location " + (int)location);
        this.location = location;
        Debug.Log("Miner: locations size " + tilingSystem.locations.Count);
        if ((int)location < tilingSystem.locations.Count)
        {
            this.currentLocation = tilingSystem.locations[(int)location];
            this.transform.position = new Vector3(currentLocation.x, currentLocation.y, 0);
        }
    }

    public void IncreaseFatigue()
    {
        this.fatigue++;
    }

    public void DecreaseFatigue()
    {
        this.fatigue--;
    }

    public bool Fatigued()
    {
        return (this.fatigue >= maxFatigue);
    }

    public void AddToGoldCarried(int amount)
    {
        this.goldCarried += amount;
        if (this.goldCarried < 0)
            this.goldCarried = 0;
    }

    public int GetGoldCarried()
    {
        return this.goldCarried;
    }

    public void SetGoldCarried(int value)
    {
        this.goldCarried = value;
    }

    public bool PocketsFull()
    {
        return (this.goldCarried >= maxGold);
    }

    public void AddToBank(int amount)
    {
        this.moneyInBank += amount;
    }

    public int GetMoneyInBank()
    {
        return this.moneyInBank;
    }

    public void RespondToBankRobbery()
    {
        Debug.Log("Miner: My Money!");
    }

    public bool Thirsty()
    {
        return (this.thirst >= maxThirst);
    }

    public void BuyAndDrinkWhiskey()
    {
        this.thirst = 0;
        this.moneyInBank -= 2;
    }

    public bool WealthyEnough()
    {
        return (this.moneyInBank >= comfortLevel);
    }
}
