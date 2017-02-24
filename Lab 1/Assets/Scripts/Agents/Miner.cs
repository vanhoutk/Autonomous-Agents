using System.Collections;
using UnityEngine;

public class Miner : Agent
{
    private const int maxGold = 3;
    private const int maxThirst = 10;
    private const int maxFatigue = 5;
    private const int comfortLevel = 5;
    private int goldCarried = 0;
    private int moneyInBank = 0;
    private int thirst = 0;
    private int fatigue = 0;

    private GameObject controller;
    private StateMachine<Miner> stateMachine;

    public AStarSearch currentPath;
    public int moveDelay = 10;
    public SquareGrid mapGrid;
    public State<Miner> previousState;
    public State<Miner> nextState;
    public Tiles destination;
    public Tiles location;
    public Tiles previousLocation;
    public TilingSystem tilingSystem;
    public Vector2 currentLocation;
    public Vector2 targetLocation;

    public void Awake()
    {
        controller = GameObject.Find("Controller");
        tilingSystem = controller.GetComponent<TilingSystem>();

        stateMachine = new StateMachine<Miner>();
        stateMachine.Init(this, GoHomeAndSleepTilRested.Instance);
        Outlaw.OnBankRobbery += RespondToBankRobbery;
    }

    public void ChangeState(State<Miner> state)
    {
        stateMachine.ChangeState(state);
    }

    public override void Update()
    {
        if(stateMachine.GetState() != MinerMovement.Instance)
            thirst++;
        stateMachine.Update();
    }

    public Tiles GetLocation()
    {
        return location;
    }

    public void FindPath(Tiles location)
    {
        Debug.Log("Miner: Start of findpath");
        mapGrid = tilingSystem.mapGrid;
        Debug.Log("Miner: MapGrid Set");
        destination = location;
        targetLocation = tilingSystem.locations[(int)location];
        var aStar = new AStarSearch(mapGrid, new Node((int)currentLocation.x, (int)currentLocation.y), new Node((int)targetLocation.x, (int)targetLocation.y));
        currentPath = aStar;
        Debug.Log("Miner: A* done...");
    }

    public void MoveAlongPath()
    {
        Node currentNode = new Node((int)currentLocation.x, (int)currentLocation.y);

        Node nextNode = new Node((int)targetLocation.x, (int)targetLocation.y);
        Node parentNode = currentPath.cameFrom[nextNode];

        while (!parentNode.Equals(currentNode))
        {
            nextNode = parentNode;
            parentNode = currentPath.cameFrom[nextNode];
        }

        currentLocation = new Vector2(nextNode.x, nextNode.y);
        transform.position = new Vector3(currentLocation.x - tilingSystem.CurrentPosition.x, currentLocation.y - tilingSystem.CurrentPosition.y, 0);
        if (currentLocation == targetLocation)
        {
            location = destination;
        }
    }

    public void ChangeLocation(Tiles location)
    {
        Debug.Log("Miner: location " + (int)location);
        this.location = location;
        Debug.Log("Miner: locations size " + tilingSystem.locations.Count);
        if ((int)location < tilingSystem.locations.Count)
        {
            currentLocation = tilingSystem.locations[(int)location];
            transform.position = new Vector3(currentLocation.x, currentLocation.y, 0);
        }
    }

    public void IncreaseFatigue()
    {
        fatigue++;
    }

    public void DecreaseFatigue()
    {
        fatigue = 0;
    }

    public bool Fatigued()
    {
        return (fatigue >= maxFatigue);
    }

    public void AddToGoldCarried(int amount)
    {
        goldCarried += amount;
        if (goldCarried < 0)
            goldCarried = 0;
    }

    public int GetGoldCarried()
    {
        return goldCarried;
    }

    public void SetGoldCarried(int value)
    {
        goldCarried = value;
    }

    public bool PocketsFull()
    {
        return (goldCarried >= maxGold);
    }

    public void AddToBank(int amount)
    {
        moneyInBank += amount;
    }

    public int GetMoneyInBank()
    {
        return moneyInBank;
    }

    public void RespondToBankRobbery()
    {
        Debug.Log("Miner: My Money!");
    }

    public bool Thirsty()
    {
        return (thirst >= maxThirst);
    }

    public void BuyAndDrinkWhiskey()
    {
        thirst = 0;
        moneyInBank -= 2;
    }

    public bool WealthyEnough()
    {
        return (moneyInBank >= comfortLevel);
    }

    public StateMachine<Miner> GetFSM()
    {
        return stateMachine;
    }
}
