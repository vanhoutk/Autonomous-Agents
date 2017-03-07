using UnityEngine;

public class Sheriff : Agent<Sheriff>
{
    private bool alive = true;
    private int goldCarried = 0;
    private GameObject controller;

    public delegate void SheriffDead(AgentTypes type);
    public static event SheriffDead OnSheriffDead;

    public delegate void SheriffShoots(int damage);
    public static event SheriffShoots OnSheriffShoots;

    public void Awake()
    {
        controller = GameObject.Find("Controller");
        tilingSystem = controller.GetComponent<TilingSystem>();

        stateMachine = new StateMachine<Sheriff>();
        stateMachine.Init(this, CheckLocation.Instance, SheriffGlobalState.Instance);
        Undertaker.OnBuriedBody += RespawnSheriff;
    }

    public void FindPath(Tiles location)
    {
        Debug.Log("Sheriff: Start of findpath");

        mapGrid = tilingSystem.mapGrid;
        Debug.Log("Sheriff: MapGrid Set");

        destination = location;
        targetLocation = tilingSystem.locations[(int)location];

        var aStar = new AStarSearch(mapGrid, new Node((int)currentLocation.x, (int)currentLocation.y), new Node((int)targetLocation.x, (int)targetLocation.y));
        currentPath = aStar;
        Debug.Log("Sheriff: A* done...");
    }

    public void ChangeLocation(Tiles location)
    {
        this.location = location;
        currentLocation = tilingSystem.locations[(int)location];
        transform.position = new Vector3(currentLocation.x - tilingSystem.CurrentPosition.x, currentLocation.y - tilingSystem.CurrentPosition.y, 0);
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

    public void RespawnSheriff(AgentTypes type)
    {
        if (type == AgentTypes.Sheriff)
        {
            alive = true;
            location = Tiles.SheriffsOffice;
            currentLocation = tilingSystem.locations[(int)location];
            transform.position = new Vector3(currentLocation.x - tilingSystem.CurrentPosition.x, currentLocation.y - tilingSystem.CurrentPosition.y, 0);
            ChangeState(WaitInSheriffOffice.Instance);
        }
        else
        {
            Debug.Log("Sheriff: I hear there's a new Outlaw in town!");
        }
    }

    public void ShotByOutlaw()
    {
        alive = false;
        if (OnSheriffDead != null)
            OnSheriffDead(AgentTypes.Sheriff);
    }

    public void ShootOutlaw()
    {
        if (OnSheriffShoots != null)
            OnSheriffShoots(1);
    }

    public StateMachine<Sheriff> GetFSM()
    {
        return stateMachine;
    }
}
