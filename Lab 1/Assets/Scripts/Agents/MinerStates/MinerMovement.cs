using UnityEngine;

public sealed class MinerMovement : State<Miner>
{
    static readonly MinerMovement instance = new MinerMovement();

    public static MinerMovement Instance
    {
        get
        {
            return instance;
        }
    }

    static MinerMovement() { }
    private MinerMovement() { }

    public override void Enter(Miner agent)
    {
        Debug.Log("Miner: Starting to walk!");
    }

    public override void Execute(Miner agent)
    {
        agent.moveDelay--;
        if (agent.moveDelay == 0)
        {
            agent.moveDelay = 10;
            Node currentNode = new Node((int)agent.currentLocation.x, (int)agent.currentLocation.y);

            Node nextNode = new Node((int)agent.targetLocation.x, (int)agent.targetLocation.y);
            Node parentNode = agent.currentPath.cameFrom[nextNode];

            while (!parentNode.Equals(currentNode))
            {
                TileSprite pathTile = agent.tilingSystem.GetTile(nextNode.x, nextNode.y);
                pathTile.SetPathColor(nextNode.x, nextNode.y, agent.tilingSystem.MapSize.y);
                nextNode = parentNode;
                parentNode = agent.currentPath.cameFrom[nextNode];
            }

            agent.currentLocation = new Vector2(nextNode.x, nextNode.y);
            agent.transform.position = new Vector3(agent.currentLocation.x - agent.tilingSystem.CurrentPosition.x, agent.currentLocation.y - agent.tilingSystem.CurrentPosition.y, 0);
            TileSprite visitedTile = agent.tilingSystem.GetTile(nextNode.x, nextNode.y);
            visitedTile.ClearPathColor(nextNode.x, nextNode.y, agent.tilingSystem.MapSize.y);
            if (agent.currentLocation == agent.targetLocation)
            {
                agent.location = agent.destination;
                agent.ChangeState(agent.nextState);
            }
        }
    }

    public override void Exit(Miner agent)
    {
        Debug.Log("Miner: Arriving at my destination!");
    }
}