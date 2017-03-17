using UnityEngine;

public class Signal
{
    public double signalStrength;
    public Sense modality;
    public SenseEvents senseEvent; //The event which caused the signal
    public Vector2 position;

    public Signal(double strength, Sense modality, SenseEvents senseEvent, Vector2 position)
    {
        signalStrength = strength;
        this.modality = modality;
        this.senseEvent = senseEvent;
        this.position = position;
    }
}
