public abstract class Sense
{
    public double inverseTransmissionSpeed;
    public double maximumRange; // Possibly not necessary
    public SenseTypes senseType;
    public Sensor sensor; //Agent that is sensed

    public virtual bool extraChecks(Signal signal, Sensor sensor)
    {
        return true;
    }
}
