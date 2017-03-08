using UnityEngine;

abstract public class Agent<T> : MonoBehaviour
{
    // Variables
    public AStarSearch currentPath;
    public bool isAlive;
    public int moveDelay = 10;
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
        Node currentNode = new Node((int)currentLocation.x, (int)currentLocation.y);
        Node nextNode = new Node((int)targetLocation.x, (int)targetLocation.y);
        Node parentNode = currentPath.cameFrom[nextNode];

        while (!nextNode.Equals(currentNode))
        {
            TileSprite pathTile = tilingSystem.GetTile(nextNode.x, nextNode.y);
            pathTile.ClearPathColor(nextNode.x, nextNode.y, tilingSystem.MapSize.y);
            nextNode = parentNode;
            parentNode = currentPath.cameFrom[nextNode];
        }
    }

    public virtual void Update()
    {
        stateMachine.Update();
    }
}