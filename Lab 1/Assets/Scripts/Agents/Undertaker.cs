using UnityEngine;

public class Undertaker : Agent<Undertaker>
{
    // Variables
    private AgentTypes bodyType;
    private GameObject controller;

    // Message Events
    public delegate void BuriedBody(AgentTypes type);
    public static event BuriedBody OnBuriedBody;

    public delegate void CollectedBody(AgentTypes type);
    public static event CollectedBody OnCollectedBody;

    // Functions
    /*
     * public StateMachine<Undertaker> GetFSM()
     * public void Awake()
     * public void ChangeLocation(Tiles location)
     * public void FindPath(Tiles location) // Find path to a particular "building"
     * public void FindPath(Vector2 location) // Find path to a grid location
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

        stateMachine = new StateMachine<Undertaker>();
        stateMachine.Init(this, WaitInUndertakers.Instance);

        // Subscribe to outlaw and sheriff death events
        Outlaw.OnOutlawDead += RespondToDeath;
        Sheriff.OnSheriffDead += RespondToDeath;
    }

    public void ChangeLocation(Tiles location)
    {
        this.location = location;
        currentLocation = tilingSystem.locations[(int)location];
        transform.position = new Vector3(currentLocation.x - tilingSystem.CurrentPosition.x, currentLocation.y - tilingSystem.CurrentPosition.y, 0);
    }

    /*public void FindPath(Tiles location)
    {
        Debug.Log("Undertaker: Start of FindPath - Building");

        mapGrid = tilingSystem.mapGrid;
        Debug.Log("Undertaker: MapGrid Set");

        destination = location;
        targetLocation = tilingSystem.locations[(int)location];

        var aStar = new AStarSearch(mapGrid, new Node((int)currentLocation.x, (int)currentLocation.y), new Node((int)targetLocation.x, (int)targetLocation.y));
        currentPath = aStar;
        Debug.Log("Undertaker: A* done...");
    }*/

    /*public void FindPath(Vector2 location)
    {
        Debug.Log("Undertaker: Start of FindPath - Grid Location");

        mapGrid = tilingSystem.mapGrid;
        Debug.Log("Undertaker: MapGrid Set");

        destination = tilingSystem.GetTile((int)location.x, (int)location.y).tileType;
        targetLocation = location;

        var aStar = new AStarSearch(mapGrid, new Node((int)currentLocation.x, (int)currentLocation.y), new Node((int)targetLocation.x, (int)targetLocation.y));
        currentPath = aStar;
        Debug.Log("Undertaker: A* done...");
    }*/

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

            GameObject outlawObject = GameObject.Find("Jesse");
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
            GameObject sheriffObject = GameObject.Find("Wyatt");
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
