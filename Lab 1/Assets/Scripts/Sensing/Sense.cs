abstract public class Sense
{
    public double inverseTransmissionSpeed;
    public SenseTypes senseType;
    public Sensor sensor; //Agent that is sensed

    public virtual bool extraChecks(Signal signal, Sensor sensor)
    {
        return true;
    }
}
