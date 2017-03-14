using UnityEngine;

public class Signal
{
    // Possibly need to add what the signal is?
    public double signalStrength;
    public Sense modality;
    public Vector2 position;

    public Signal(double strength, Sense modality, Vector2 position)
    {
        signalStrength = strength;
        this.modality = modality;
        this.position = position;
    }
}
