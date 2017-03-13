using UnityEngine;

abstract public class Agent<T> : MonoBehaviour
{
    // Variables
    public AStarSearch currentPath;
    public bool isAlive;
    public int moveDelay = 10;
    public SenseManager senseManager;
    public SquareGrid mapGrid;
    public State<T> nextState;
    public State<T> previousState;
    public StateMachine<T> stateMachine;
    public Tiles destination;
    public Tiles location;
    public Tiles previousLocation;
    public TilingSystem tilingSystem;
    public Vector2 currentLocation;
    public Vector2 targetLocation;

    // Functions
    /*
     * public Tiles GetLocation()
     * public void ChangeState(State<T> state)
     * public void ClearCurrentPath()
     * public void FindPath(Tiles location)
     * public void FindPath(Vector2 location)
     * public void Update()
     */

    public Tiles GetLocation()
    {
        return location;
    }

    public void ChangeState(State<T> state)
    {
        stateMachine.ChangeState(state);
    }

    public void ClearCurrentPath()
    {
        //Node currentNode = new Node((int)currentLocation.x, (int)currentLocation.y);
        Node currentNode = mapGrid.nodeSet[new Coordinates((int)currentLocation.x, (int)currentLocation.y)];
        //Node nextNode = new Node((int)targetLocation.x, (int)targetLocation.y);
        Node nextNode = mapGrid.nodeSet[new Coordinates((int)targetLocation.x, (int)targetLocation.y)];
        Node parentNode = currentPath.cameFrom[nextNode];


        while (!nextNode.Equals(currentNode))
        {
            TileSprite pathTile = tilingSystem.GetTile(nextNode.coordinates.x, nextNode.coordinates.y);
            pathTile.ClearPathColor(nextNode.coordinates.x, nextNode.coordinates.y, tilingSystem.MapSize.y);
            nextNode = parentNode;
            parentNode = currentPath.cameFrom[nextNode];
        }
    }

    public void FindPath(Tiles location)
    {
        mapGrid = tilingSystem.mapGrid;
        destination = location;
        targetLocation = tilingSystem.locations[(int)location];
        //var aStar = new AStarSearch(mapGrid, new Node((int)currentLocation.x, (int)currentLocation.y), new Node((int)targetLocation.x, (int)targetLocation.y));
        var aStar = new AStarSearch(mapGrid, mapGrid.nodeSet[new Coordinates((int)currentLocation.x, (int)currentLocation.y)], mapGrid.nodeSet[new Coordinates((int)targetLocation.x, (int)targetLocation.y)]);
        currentPath = aStar;
    }

    public void FindPath(Vector2 location)
    {
        mapGrid = tilingSystem.mapGrid;
        destination = tilingSystem.GetTile((int)location.x, (int)location.y).tileType;
        targetLocation = location;
        //var aStar = new AStarSearch(mapGrid, new Node((int)currentLocation.x, (int)currentLocation.y), new Node((int)targetLocation.x, (int)targetLocation.y));
        var aStar = new AStarSearch(mapGrid, mapGrid.nodeSet[new Coordinates((int)currentLocation.x, (int)currentLocation.y)], mapGrid.nodeSet[new Coordinates((int)targetLocation.x, (int)targetLocation.y)]);
        currentPath = aStar;
    }

    public virtual void Update()
    {
        stateMachine.Update();
    }
}