using UnityEngine;

public class Outlaw : Agent
{
    private AStarSearch currentPath;
    private int goldCarried = 0;
    private GameObject controller;
    private StateMachine<Outlaw> stateMachine;
    private Tiles location;
    private Tiles destination;

    public delegate void BankRobbery();
    public static event BankRobbery OnBankRobbery;
    public SquareGrid mapGrid;
    public TilingSystem tilingSystem;
    public Vector2 currentLocation;
    public Vector2 targetLocation;

    public void Awake()
    {
        controller = GameObject.Find("Controller");
        tilingSystem = controller.GetComponent<TilingSystem>();

        stateMachine = new StateMachine<Outlaw>();
        stateMachine.Init(this, LurkInCamp.Instance, OutlawGlobalState.Instance);
    }

    public void ChangeState(State<Outlaw> state)
    {
        stateMachine.ChangeState(state);
    }

    public override void Update()
    {
        stateMachine.Update();
    }

    public Tiles GetLocation()
    {
        return location;
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

    public void MoveAlongPath()
    {
        Node currentNode = new Node((int)currentLocation.x, (int)currentLocation.y);

        Node nextNode = new Node((int)targetLocation.x, (int)targetLocation.y);
        Node parentNode = currentPath.cameFrom[nextNode];

        while(!parentNode.Equals(currentNode))
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

        /*Node nextNode = this.currentPath.cameFrom[new Node((int)this.currentLocation.x, (int)this.currentLocation.y)];
        this.currentLocation = new Vector2(nextNode.x, nextNode.y);
        transform.position = new Vector3(currentLocation.x - tilingSystem.CurrentPosition.x, currentLocation.y - tilingSystem.CurrentPosition.y, 0);
        if (this.currentLocation == this.targetLocation)
        {
            this.location = this.destination;
        }*/
        //ChangeLocation(destination);
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

    public void RobBank()
    {
        goldCarried += Random.Range(1, 10);
        if(OnBankRobbery != null)
            OnBankRobbery();
    }

    public StateMachine<Outlaw> GetFSM()
    {
        return stateMachine;
    }
}
