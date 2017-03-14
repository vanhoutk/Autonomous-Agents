using UnityEngine;

public class Sight : Sense
{
    public new double inverseTransmissionSpeed = 0;
    public new SenseTypes senseType = SenseTypes.Sight;
    //RaycastHit2D hit = Physics2D.RaycastAll(Vector2 agentLocation, Vector2 destinationLocation, float distanceAgentCanSee);
}