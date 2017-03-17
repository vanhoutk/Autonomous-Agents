using System.Collections.Generic;
using UnityEngine;

public class Sensor
{
    public AgentTypes agentType;
    public Dictionary<SenseTypes, double> thresholds;
    public GameObject gameObject;

    public bool DetectsModality(SenseTypes modality)
    {
        if (thresholds.ContainsKey(modality))
            return true;
        else
            return false;
    }

    public Sensor(AgentTypes agentType, GameObject gameObject, List<SenseTypes> modalities, List<double> thresholds)
    {
        this.agentType = agentType;
        this.gameObject = gameObject;
        this.thresholds = new Dictionary<SenseTypes, double>();
        for(int i = 0; i < modalities.Count; i++)
        {
            this.thresholds.Add(modalities[i], thresholds[i]);
        }
    }
}