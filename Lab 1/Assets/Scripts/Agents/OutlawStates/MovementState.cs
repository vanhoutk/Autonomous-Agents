using UnityEngine;

public sealed class MovementState : State<Outlaw>
{
    static readonly MovementState instance = new MovementState();

    public static MovementState Instance
    {
        get
        {
            return instance;
        }
    }

    static MovementState() { }
    private MovementState() { }

    public override void Enter(Outlaw agent)
    {
        Debug.Log("Outlaw: Starting to walk!");
    }

    public override void Execute(Outlaw agent)
    {
        agent.moveDelay--;
        if(agent.moveDelay == 0)
        {
            agent.moveDelay = 5;
            Node currentNode = new Node((int)agent.currentLocation.x, (int)agent.currentLocation.y);

            Node nextNode = new Node((int)agent.targetLocation.x, (int)agent.targetLocation.y);
            Node parentNode = agent.currentPath.cameFrom[nextNode];

            while (!parentNode.Equals(currentNode))
            {
                nextNode = parentNode;
                parentNode = agent.currentPath.cameFrom[nextNode];
            }

            agent.currentLocation = new Vector2(nextNode.x, nextNode.y);
            agent.transform.position = new Vector3(agent.currentLocation.x - agent.tilingSystem.CurrentPosition.x, agent.currentLocation.y - agent.tilingSystem.CurrentPosition.y, 0);
            if (agent.currentLocation == agent.targetLocation)
            {
                agent.location = agent.destination;
                agent.ChangeState(agent.nextState);
            }
        }
    }

    public override void Exit(Outlaw agent)
    {
        Debug.Log("Outlaw: Arriving at my destination!");
    }
}