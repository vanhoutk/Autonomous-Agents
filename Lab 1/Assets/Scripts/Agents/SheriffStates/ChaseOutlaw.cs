using System;
using UnityEngine;

public sealed class ChaseOutlaw : State<Sheriff>
{
    static public double Distance(Vector2 a, Vector2 b)
    {
        return Math.Abs(a.x - b.x) + Math.Abs(a.y - b.y);
    }

    static readonly ChaseOutlaw instance = new ChaseOutlaw();

    public static ChaseOutlaw Instance
    {
        get
        {
            return instance;
        }
    }

    static ChaseOutlaw() { }
    private ChaseOutlaw() { }

    public override void Enter(Sheriff agent)
    {
        agent.Log("Gonna catch that outlaw!");
    }

    public override void Execute(Sheriff agent)
    {
        GameObject outlawObject = GameObject.Find(Outlaw.agentName);
        if(outlawObject != null)
        {
            Outlaw outlaw = outlawObject.GetComponent<Outlaw>();
            if (Distance(agent.currentLocation, outlaw.currentLocation) >= 1.0 && outlaw.isAlive)
            {
                if (agent.currentPath != null)
                    agent.ClearCurrentPath();

                agent.FindPath(outlaw.currentLocation);
                agent.nextState = FightOutlaw.Instance;

                agent.moveDelay -= 2;
                if (agent.moveDelay <= 0)
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
                }
            }
            else if (outlaw.isAlive)
            {
                agent.YellAtOutlaw();
                agent.ChangeState(FightOutlaw.Instance);
            }
            else
            {
                agent.FindPath(Tiles.SheriffsOffice);
                agent.nextState = WaitInSheriffOffice.Instance;
                agent.ChangeState(Movement<Sheriff>.Instance);
            }
        }
        else
        {
            agent.Log("ERROR: Outlaw does not exist");
        }
    }

    public override void Exit(Sheriff agent)
    {
    }
}
