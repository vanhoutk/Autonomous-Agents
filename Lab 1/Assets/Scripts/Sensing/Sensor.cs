using System.Collections.Generic;
using UnityEngine;

public class Sensor
{
    public AgentTypes agentType;
    public Dictionary<SenseTypes, double> thresholds;
    public GameObject gameObject; // Can get the transform from this
    public List<SenseTypes> modalities;
    public string agentName;
    //public Vector3 position;

    public bool DetectsModality(SenseTypes modality)
    {
        if (modalities.Contains(modality))
            return true;
        else
            return false;
        //Agent<agentType> agent = gameObject.GetComponent<Agent<agentType>>();
        //return true;
    }

    //public void Notify(Signal signal)
    //{
    //    if(agentType == AgentTypes.Miner)
    //    {

    //    }
    //}

    public Sensor(AgentTypes agentType, GameObject gameObject, List<SenseTypes> modalities, List<double> thresholds, string agentName)
    {
        this.agentType = agentType;
        this.gameObject = gameObject;
        this.modalities = modalities;
        this.agentName = agentName;
        this.thresholds = new Dictionary<SenseTypes, double>();
        for(int i = 0; i < modalities.Count; i++)
        {
            this.thresholds.Add(modalities[i], thresholds[i]);
        }
    }
}