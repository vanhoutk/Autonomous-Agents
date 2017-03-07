using UnityEngine;

public class Outlaw : Agent<Outlaw>
{
    private bool alive = true;
    private int goldCarried = 0;
    private GameObject controller;

    public delegate void BankRobbery();
    public static event BankRobbery OnBankRobbery;

    public delegate void OutlawDead(AgentTypes type);
    public static event OutlawDead OnOutlawDead;

    public void Awake()
    {
        controller = GameObject.Find("Controller");
        tilingSystem = controller.GetComponent<TilingSystem>();

        stateMachine = new StateMachine<Outlaw>();
        stateMachine.Init(this, LurkInCamp.Instance, OutlawGlobalState.Instance);
        Undertaker.OnBuriedBody += RespawnOutlaw;
        Sheriff.OnSheriffShoots += TakeDamage;
    }

    public void ChangeLocation(Tiles location)
    {
        this.location = location;
        currentLocation = tilingSystem.locations[(int)location];
        transform.position = new Vector3(currentLocation.x - tilingSystem.CurrentPosition.x, currentLocation.y - tilingSystem.CurrentPosition.y, 0);
    }

    public void FindPath(Tiles location)
    {
        Debug.Log("Outlaw: Start of findpath");
        mapGrid = tilingSystem.mapGrid;
        Debug.Log("Outlaw: MapGrid Set");
        destination = location;
        targetLocation = tilingSystem.locations[(int)location];
        var aStar = new AStarSearch(mapGrid, new Node((int)currentLocation.x, (int)currentLocation.y), new Node((int)targetLocation.x, (int)targetLocation.y));
        currentPath = aStar;
        Debug.Log("Outlaw: A* done...");
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

    public void RobBank()
    {
        goldCarried += Random.Range(1, 10);
        if(OnBankRobbery != null)
            OnBankRobbery();
    }

    public void RespawnOutlaw(AgentTypes type)
    {
        if(type == AgentTypes.Outlaw)
        {
            alive = true;
            location = Tiles.OutlawCamp;
            currentLocation = tilingSystem.locations[(int)location];
            transform.position = new Vector3(currentLocation.x - tilingSystem.CurrentPosition.x, currentLocation.y - tilingSystem.CurrentPosition.y, 0);
            ChangeState(LurkInCamp.Instance);
        }
        else
        {
            Debug.Log("Outlaw: I hear there's a new Sheriff in town!");
        }

    }

    public void TakeDamage(int damage)
    {
        if(damage >= 0)
            ShotBySheriff();
    }

    public void ShotBySheriff()
    {
        alive = false;
        if (OnOutlawDead != null)
            OnOutlawDead(AgentTypes.Outlaw);
    }

    public StateMachine<Outlaw> GetFSM()
    {
        return stateMachine;
    }
}
