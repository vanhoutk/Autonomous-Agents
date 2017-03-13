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
     * public void ChangeLocation(Tiles location) // Not used anymore
     * public void FindPath(Tiles location)
     * public void RespondToBankRobbery()
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

        stateMachine = new StateMachine<Miner>();
        stateMachine.Init(this, GoHomeAndSleepTilRested.Instance);
        Outlaw.OnBankRobbery += RespondToBankRobbery;
    }

    public void ChangeLocation(Tiles location)
    {
        this.location = location;
        currentLocation = tilingSystem.locations[(int)location];
        transform.position = new Vector3(currentLocation.x - tilingSystem.CurrentPosition.x, currentLocation.y - tilingSystem.CurrentPosition.y, 0);
    }

    /*public void FindPath(Tiles location)
    {
        Debug.Log("Miner: Start of findpath");
        mapGrid = tilingSystem.mapGrid;
        Debug.Log("Miner: MapGrid Set");
        destination = location;
        targetLocation = tilingSystem.locations[(int)location];
        var aStar = new AStarSearch(mapGrid, new Node((int)currentLocation.x, (int)currentLocation.y), new Node((int)targetLocation.x, (int)targetLocation.y));
        currentPath = aStar;
        Debug.Log("Miner: A* done...");
    }*/

    public void RespondToBankRobbery(int amount)
    {
        if (amount > moneyInBank)
            moneyInBank = 0;
        else
            moneyInBank -= amount;
        Debug.Log("Miner: My Money!");
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
