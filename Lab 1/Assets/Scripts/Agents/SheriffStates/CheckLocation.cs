using System;
using UnityEngine;

public sealed class CheckLocation : State<Sheriff>
{
    static public double Distance(Vector2 a, Vector2 b)
    {
        return Math.Abs(a.x - b.x) + Math.Abs(a.y - b.y);
    }

    static readonly CheckLocation instance = new CheckLocation();

    public static CheckLocation Instance
    {
        get
        {
            return instance;
        }
    }

    static CheckLocation() { }
    private CheckLocation() { }

    public override void Enter(Sheriff agent)
    {
        Debug.Log("Sheriff: Arrived at a location!");
    }

    public override void Execute(Sheriff agent)
    {
        GameObject outlawObject = GameObject.Find(Outlaw.agentName);
        if(outlawObject != null)
        {
            Outlaw outlaw = outlawObject.GetComponent<Outlaw>();
            if (Distance(agent.currentLocation, outlaw.currentLocation) <= 1.0 && outlaw.isAlive)
            {
                agent.YellAtOutlaw();

                if (agent.currentPath != null)
                    agent.ClearCurrentPath();

                agent.ChangeState(FightOutlaw.Instance);
            }
            else if (UnityEngine.Random.Range(0.0f, 1.0f) < 0.2f)
            {
                Debug.Log("Sheriff: Going to check the next location!");
                int nextLocation;
                do
                {
                    nextLocation = UnityEngine.Random.Range((int)Tiles.Shack, (int)Tiles.NUMBER_OF_TILES);
                } while (nextLocation == (int)Tiles.OutlawCamp || nextLocation == (int)agent.location);

                agent.FindPath((Tiles)nextLocation);
                agent.nextState = Instance;
                agent.ChangeState(Movement<Sheriff>.Instance);
            }
            else
            {
                Debug.Log("Sheriff: Just checking out this location!");
            }
        }
        else if (UnityEngine.Random.Range(0.0f, 1.0f) < 0.2f)
        {
            Debug.Log("Sheriff: Going to check the next location!");
            int nextLocation;
            do
            {
                nextLocation = UnityEngine.Random.Range((int)Tiles.Shack, (int)Tiles.NUMBER_OF_TILES);
            } while (nextLocation == (int)Tiles.OutlawCamp || nextLocation == (int)agent.location);

            agent.FindPath((Tiles)nextLocation);
            agent.nextState = Instance;
            agent.ChangeState(Movement<Sheriff>.Instance);
        }
        else
        {
            Debug.Log("Sheriff: Just checking out this location!");
        }
    }

    public override void Exit(Sheriff agent)
    {
        Debug.Log("Sheriff: Leaving this location.");
    }
}
