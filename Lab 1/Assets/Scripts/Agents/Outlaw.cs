using UnityEngine;

public class Outlaw : Agent
{
    private int goldCarried = 0;
    private GameObject controller;
    private StateMachine<Outlaw> stateMachine;
    private Tiles location;

    public delegate void BankRobbery();
    public static event BankRobbery OnBankRobbery;
    public TilingSystem tilingSystem;
    public Vector2 currentLocation;

    public void Awake()
    {
        this.controller = GameObject.Find("Controller");
        this.tilingSystem = controller.GetComponent<TilingSystem>();

        this.stateMachine = new StateMachine<Outlaw>();
        this.stateMachine.Init(this, LurkInCamp.Instance, OutlawGlobalState.Instance);
    }

    public void ChangeState(State<Outlaw> state)
    {
        this.stateMachine.ChangeState(state);
    }

    public override void Update()
    {
        this.stateMachine.Update();
    }

    public Tiles GetLocation()
    {
        return this.location;
    }

    public void ChangeLocation(Tiles location)
    {
        this.location = location;
        this.currentLocation = tilingSystem.locations[(int)location];
        this.transform.position = new Vector3(currentLocation.x, currentLocation.y, 0);
    }

    public void AddToGoldCarried(int amount)
    {
        this.goldCarried += amount;
        if (this.goldCarried < 0)
            this.goldCarried = 0;
    }

    public int GetGoldCarried()
    {
        return this.goldCarried;
    }

    public void SetGoldCarried(int value)
    {
        this.goldCarried = value;
    }

    public void RobBank()
    {
        this.goldCarried += Random.Range(1, 10);
        if(OnBankRobbery != null)
            OnBankRobbery();
    }

    public StateMachine<Outlaw> GetFSM()
    {
        return this.stateMachine;
    }
}
