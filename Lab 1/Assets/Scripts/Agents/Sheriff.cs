using UnityEngine;

public class Sheriff : Agent<Sheriff>
{
    // Variables
    private GameObject controller;

    // Messaging Events
    public delegate void EncounterOutlaw();
    public static event EncounterOutlaw OnEncounterOutlaw;

    public delegate void SheriffDead(AgentTypes type);
    public static event SheriffDead OnSheriffDead;

    public delegate void SheriffShoots(int damage);
    public static event SheriffShoots OnSheriffShoots;

    // Functions
    /* 
     * public StateMachine<Sheriff> GetFSM()
     * public void Awake()
     * public void ChangeLocation(Tiles location)
     * public void FindPath(Tiles location)
     *
     * public void DespawnSheriff(AgentTypes type)
     * public void RespawnSheriff(AgentTypes type)
     * public void ShootOutlaw()
     * public void ShotByOutlaw()
     * public void YellAtOutlaw()
     */

    public StateMachine<Sheriff> GetFSM()
    {
        return stateMachine;
    }

    public void Awake()
    {
        isAlive = true;

        controller = GameObject.Find("Controller");
        tilingSystem = controller.GetComponent<TilingSystem>();

        stateMachine = new StateMachine<Sheriff>();
        stateMachine.Init(this, CheckLocation.Instance, SheriffGlobalState.Instance);

        // Subscribe to the undertakers messages
        Undertaker.OnBuriedBody += RespawnSheriff;
        Undertaker.OnCollectedBody += DespawnSheriff;
    }

    public void ChangeLocation(Tiles location)
    {
        this.location = location;
        currentLocation = tilingSystem.locations[(int)location];
        transform.position = new Vector3(currentLocation.x - tilingSystem.CurrentPosition.x, currentLocation.y - tilingSystem.CurrentPosition.y, 0);
    }

    /*public void FindPath(Tiles location)
    {
        Debug.Log("Sheriff: Start of findpath");

        mapGrid = tilingSystem.mapGrid;
        Debug.Log("Sheriff: MapGrid Set");

        destination = location;
        targetLocation = tilingSystem.locations[(int)location];

        var aStar = new AStarSearch(mapGrid, new Node((int)currentLocation.x, (int)currentLocation.y), new Node((int)targetLocation.x, (int)targetLocation.y));
        currentPath = aStar;
        Debug.Log("Sheriff: A* done...");
    }*/

    public void DespawnSheriff(AgentTypes type)
    {
        if (type == AgentTypes.Sheriff)
        {
            GameObject sheriffObject = GameObject.Find("Wyatt");
            SpriteRenderer sheriffRenderer = sheriffObject.GetComponent<SpriteRenderer>();
            sheriffRenderer.enabled = false;
        }
    }

    public void RespawnSheriff(AgentTypes type)
    {
        if (type == AgentTypes.Sheriff)
        {
            GameObject sheriffObject = GameObject.Find("Wyatt");
            SpriteRenderer sheriffRenderer = sheriffObject.GetComponent<SpriteRenderer>();
            sheriffRenderer.enabled = true;

            isAlive = true;

            ChangeLocation(Tiles.SheriffsOffice);
            ChangeState(WaitInSheriffOffice.Instance);
        }
        else
        {
            Debug.Log("Sheriff: I hear there's a new Outlaw in town!");

            if(stateMachine.GetState() != CheckLocation.Instance)
                ChangeState(CheckLocation.Instance);
        }
    }

    public void ShootOutlaw()
    {
        if (OnSheriffShoots != null)
            OnSheriffShoots(1);
    }

    public void ShotByOutlaw()
    {
        isAlive = false;
        if (OnSheriffDead != null)
            OnSheriffDead(AgentTypes.Sheriff);
    }

    public void YellAtOutlaw()
    {
        if (OnEncounterOutlaw != null)
            OnEncounterOutlaw();
    }
}
