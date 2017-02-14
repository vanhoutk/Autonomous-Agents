using UnityEngine;

public sealed class QuenchThirst : State<Miner>
{

    static readonly QuenchThirst instance = new QuenchThirst();

    public static QuenchThirst Instance
    {
        get
        {
            return instance;
        }
    }

    static QuenchThirst() { }
    private QuenchThirst() { }

    public override void Enter(Miner agent)
    {
        // If the miner is not already located at the gold mine, he must
        // change location to the gold mine
        if (agent.GetLocation() != Tiles.Saloon)
        {
            Debug.Log("Miner: Boy, ah sure is thirsty! Walking to the saloon");
            agent.ChangeLocation(Tiles.Saloon);
        }
    }

    public override void Execute(Miner agent)
    {
        if (agent.Thirsty())
        {
            agent.BuyAndDrinkWhiskey();
            Debug.Log("Miner: That's mighty fine sippin liquer!");
            agent.ChangeState(EnterMineAndDigForNugget.Instance);
        }
        else
        {
            agent.DecreaseFatigue();
            Debug.Log("ERROR! ERROR! ERROR!");
        }
    }

    public override void Exit(Miner agent)
    {
        Debug.Log("Miner: Leavin' the saloon, feelin' good!");
    }
}
