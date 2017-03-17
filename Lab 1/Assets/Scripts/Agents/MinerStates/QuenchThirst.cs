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
        agent.Log("Boy, ah sure is thirsty! I'm finally at the saloon.");
    }

    public override void Execute(Miner agent)
    {
        if (agent.Thirsty())
        {
            agent.BuyAndDrinkWhiskey();
            agent.Log("That's mighty fine sippin liquer! Now back to work for me!");
            agent.FindPath(Tiles.GoldMine);
            agent.nextState = EnterMineAndDigForNugget.Instance;
            agent.ChangeState(Movement<Miner>.Instance);
        }
        else
        {
            agent.Log("ERROR! ERROR! ERROR!");
        }
    }

    public override void Exit(Miner agent)
    {
        agent.Log("Leavin' the saloon, feelin' good!");
    }
}
