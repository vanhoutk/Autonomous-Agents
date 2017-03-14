using UnityEngine;

public class Outlaw : Agent<Outlaw>
{
    // Variables
    private int goldCarried = 0;
    private GameObject controller;

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
     * public void FindPath(Tiles location)
     * public void Start()
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

    /*public void FindPath(Tiles location)
    {
        Debug.Log("Outlaw: Start of findpath");
        mapGrid = tilingSystem.mapGrid;
        Debug.Log("Outlaw: MapGrid Set");
        destination = location;
        targetLocation = tilingSystem.locations[(int)location];
        var aStar = new AStarSearch(mapGrid, new Node((int)currentLocation.x, (int)currentLocation.y), new Node((int)targetLocation.x, (int)targetLocation.y));
        currentPath = aStar;
        Debug.Log("Outlaw: A* done...");
    }*/

    public void Start()
    {
        ChangeLocation(Tiles.OutlawCamp);
    }

    public void DespawnOutlaw(AgentTypes type)
    {
        if (type == AgentTypes.Outlaw)
        {
            GameObject outlawObject = GameObject.Find("Jesse");
            SpriteRenderer outlawRenderer = outlawObject.GetComponent<SpriteRenderer>();
            outlawRenderer.enabled = false;
        }
    }

    public void RespawnOutlaw(AgentTypes type)
    {
        if (type == AgentTypes.Outlaw)
        {
            GameObject outlawObject = GameObject.Find("Jesse");
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
