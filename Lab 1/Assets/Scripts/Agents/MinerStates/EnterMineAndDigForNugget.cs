using UnityEngine;

public sealed class EnterMineAndDigForNugget : State<Miner>
{

    static readonly EnterMineAndDigForNugget instance = new EnterMineAndDigForNugget();

    public static EnterMineAndDigForNugget Instance
    {
        get
        {
            return instance;
        }
    }

    static EnterMineAndDigForNugget() { }
    private EnterMineAndDigForNugget() { }

    public override void Enter(Miner agent)
    {
        // If the miner is not already located at the gold mine, he must
        // change location to the gold mine
        //if(agent.GetLocation() != Tiles.GoldMine)
        //{
            Debug.Log("Miner: Walkin' to the gold mine...");
        //    agent.ChangeLocation(Tiles.GoldMine);
        //}
    }

    public override void Execute(Miner agent)
    {
        // The miner digs for gold until he is carrying in excess of MaxNuggets.
        // If he gets thirsty during his digging he stops work and
        // changes state to go to the saloon for a whiskey.
        agent.AddToGoldCarried(1);
        // Diggin' is hard work
        agent.IncreaseFatigue();
        Debug.Log("Miner: Pickin' up a nugget");
        // If enough gold mined, go and put it in the bank
        if (agent.PocketsFull())
        {
            agent.FindPath(Tiles.Bank);
            agent.nextState = VisitBankAndDepositGold.Instance;
            agent.ChangeState(Movement<Miner>.Instance);
            //agent.ChangeState(VisitBankAndDepositGold.Instance);
        }
        // If thirsty go and get a whiskey
        if (agent.Thirsty())
        {
            agent.FindPath(Tiles.Saloon);
            agent.nextState = QuenchThirst.Instance;
            agent.ChangeState(Movement<Miner>.Instance);
            //agent.ChangeState(QuenchThirst.Instance);
        }

        if (agent.Fatigued())
        {
            agent.FindPath(Tiles.Shack);
            agent.nextState = GoHomeAndSleepTilRested.Instance;
            agent.ChangeState(Movement<Miner>.Instance);
        }
    }

    public override void Exit(Miner agent)
    {
        //agent.previousState = Instance;
        //agent.previousLocation = agent.location;
        Debug.Log("Miner: Ah'm leavin' the gold mine with mah pockets full o' sweet gold");
    }
}
