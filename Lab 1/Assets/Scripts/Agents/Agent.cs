using UnityEngine;
using System.Collections;

abstract public class Agent<T> : MonoBehaviour
{
    public AStarSearch currentPath;
    public int moveDelay = 10;
    public State<T> previousState;
    public State<T> nextState;
    public StateMachine<T> stateMachine;
    public Tiles destination;
    public Tiles location;
    public Tiles previousLocation;
    public TilingSystem tilingSystem;
    public Vector2 currentLocation;
    public Vector2 targetLocation;

    public void Update()
    {
        stateMachine.Update();
    }

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
}