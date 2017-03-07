using UnityEngine;

public class Undertaker : Agent<Undertaker>
{
    private AgentTypes bodyType;
    private GameObject controller;

    public delegate void BuriedBody(AgentTypes type);
    public static event BuriedBody OnBuriedBody;

    public void Awake()
    {
        controller = GameObject.Find("Controller");
        tilingSystem = controller.GetComponent<TilingSystem>();

        stateMachine = new StateMachine<Undertaker>();
        stateMachine.Init(this, WaitInUndertakers.Instance);
        Outlaw.OnOutlawDead += RespondToDeath;
        Sheriff.OnSheriffDead += RespondToDeath;
    }

    public void FindPath(Tiles location)
    {
        Debug.Log("Undertaker: Start of findpath");

        mapGrid = tilingSystem.mapGrid;
        Debug.Log("Undertaker: MapGrid Set");

        destination = location;
        targetLocation = tilingSystem.locations[(int)location];

        var aStar = new AStarSearch(mapGrid, new Node((int)currentLocation.x, (int)currentLocation.y), new Node((int)targetLocation.x, (int)targetLocation.y));
        currentPath = aStar;
        Debug.Log("Undertaker: A* done...");
    }

    public void FindPath(Vector2 location)
    {
        Debug.Log("Undertaker: Start of findpath");

        mapGrid = tilingSystem.mapGrid;
        Debug.Log("Undertaker: MapGrid Set");

        destination = tilingSystem.GetTile((int)location.x, (int)location.y).tileType;
        targetLocation = location;

        var aStar = new AStarSearch(mapGrid, new Node((int)currentLocation.x, (int)currentLocation.y), new Node((int)targetLocation.x, (int)targetLocation.y));
        currentPath = aStar;
        Debug.Log("Undertaker: A* done...");
    }

    public void ChangeLocation(Tiles location)
    {
        this.location = location;
        currentLocation = tilingSystem.locations[(int)location];
        transform.position = new Vector3(currentLocation.x - tilingSystem.CurrentPosition.x, currentLocation.y - tilingSystem.CurrentPosition.y, 0);
    }

    public void BuryBody()
    {
        if(OnBuriedBody != null)
            OnBuriedBody(bodyType);
    }

    public void RespondToDeath(AgentTypes type)
    {
        bodyType = type;

        if(type == AgentTypes.Outlaw)
        {
            Debug.Log("Undertaker: The Sheriff has killed the Outlaw!");
            GameObject outlawObject = GameObject.Find("Jesse");
            Outlaw outlaw = outlawObject.GetComponent<Outlaw>();
            FindPath(outlaw.currentLocation);
        }
        else
        {
            Debug.Log("Undertaker: The Outlaw has killed the Sheriff!");
            GameObject sheriffObject = GameObject.Find("Wyatt");
            Sheriff sheriff = sheriffObject.GetComponent<Sheriff>();
            FindPath(sheriff.currentLocation);
        }

        nextState = CollectBody.Instance;
        ChangeState(Movement<Undertaker>.Instance);
        Debug.Log("Undertaker: Going to pick up the outlaw's body!");
    }

    public StateMachine<Undertaker> GetFSM()
    {
        return stateMachine;
    }
}
