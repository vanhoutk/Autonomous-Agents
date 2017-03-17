using UnityEngine;

public sealed class Movement<T>: State<T> where T : Agent<T>
{
    static readonly Movement<T> instance = new Movement<T>();

    public static Movement<T> Instance
    {
        get
        {
            return instance;
        }
    }

    static Movement() { }
    private Movement() { }

    public override void Enter(T agent)
    {
        agent.Log("Starting to walk!");
    }

    public override void Execute(T agent)
    {
        agent.moveDelay--;
        if (agent.moveDelay == 0)
        {
            agent.moveDelay = 10;
            
            Node currentNode = agent.mapGrid.nodeSet[new Coordinates((int)agent.currentLocation.x, (int)agent.currentLocation.y)];
            Node nextNode = agent.mapGrid.nodeSet[new Coordinates((int)agent.targetLocation.x, (int)agent.targetLocation.y)];
            Node parentNode = agent.currentPath.cameFrom[nextNode];

            while (!parentNode.Equals(currentNode))
            {
                TileSprite pathTile = agent.tilingSystem.GetTile(nextNode.coordinates.x, nextNode.coordinates.y);
                pathTile.SetPathColor(nextNode.coordinates.x, nextNode.coordinates.y, agent.tilingSystem.MapSize.y);
                nextNode = parentNode;
                parentNode = agent.currentPath.cameFrom[nextNode];
            }

            agent.currentLocation = new Vector2(nextNode.coordinates.x, nextNode.coordinates.y);
            agent.transform.position = new Vector3((agent.currentLocation.x - agent.tilingSystem.CurrentPosition.x) * agent.tilingSystem.tileSize, (agent.currentLocation.y - agent.tilingSystem.CurrentPosition.y) * agent.tilingSystem.tileSize, 0);

            TileSprite visitedTile = agent.tilingSystem.GetTile(nextNode.coordinates.x, nextNode.coordinates.y);
            visitedTile.ClearPathColor(nextNode.coordinates.x, nextNode.coordinates.y, agent.tilingSystem.MapSize.y);

            if (agent.currentLocation == agent.targetLocation)
            {
                agent.location = agent.destination;
                agent.ChangeState(agent.nextState);
            }
        }
    }

    public override void Exit(T agent)
    {
        agent.Log("Arriving at my destination!");
    }
}