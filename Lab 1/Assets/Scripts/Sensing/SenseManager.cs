using System;
using System.Collections.Generic;
using UnityEngine;

// Holds a record in the notification queue, ready to notify the sensor at the correct time.
public struct Notification
{
    public double time;
    public Sensor sensor;
    public Signal signal;
}

public class SenseManager : MonoBehaviour
{
    public List<Sensor> sensors; // Holds the list of sensors
    private GameObject controller;
    private Queue<Notification> notificationQueue; // Holds a queue of notifcations waiting to be honoured
    private TilingSystem tilingSystem;

    public delegate void MinerNotify(Signal signal);
    public static event MinerNotify NotifyMiner;

    public delegate void OutlawNotify(Signal signal);
    public static event OutlawNotify NotifyOutlaw;

    public delegate void SheriffNotify(Signal signal);
    public static event SheriffNotify NotifySheriff;

    public delegate void UndertakerNotify(Signal signal);
    public static event UndertakerNotify NotifyUndertaker;

    void addSignal(Signal signal)
    {
        // Aggregation Phase
        List<Sensor> valid_sensors = new List<Sensor>();
        foreach(Sensor sensor in sensors)
        {
            // Check if sensor detects the signal
            if (!sensor.DetectsModality(signal.modality.senseType))
                continue;

            // Check if distance of signal exceeds the max range of the modality
            //distance = distance(signal.position, sensor.position)
            // if(signal.modality.maximumRange < distance)
            // continue;

            // Find the intensity of the signal and check threshold
            // intensity = signal.strength * pow(signal.modality.attenuation, distance)
            // if(intensity < sensor.threshold)
            // continue;

            // Probably only need to do intensity as signal.strength - the cost in the astar search and if less than threshold
            Vector3 current_position = sensor.gameObject.transform.position;
            Vector2 current_location = new Vector2(current_position.x + tilingSystem.CurrentPosition.x, current_position.y + tilingSystem.CurrentPosition.y);
            SquareGrid mapGrid = tilingSystem.mapGrid;
            // Path from signal to sensor
            AStarSearch sensePath = new AStarSearch(mapGrid, mapGrid.nodeSet[new Coordinates((int)signal.position.x, (int)signal.position.y)], mapGrid.nodeSet[new Coordinates((int)current_location.x, (int)current_location.y)], signal.modality.senseType);
            double pathCost = sensePath.gScore[mapGrid.nodeSet[new Coordinates((int)current_location.x, (int)current_location.y)]];
            double intensity = Math.Max(signal.signalStrength - pathCost, 0);
            //if (intensity < sensor.threshold)
            //    continue;

            // Perform additional modality specific checks
            if(!signal.modality.extraChecks(signal, sensor))
                continue;

            double current_time = (DateTime.Now.ToUniversalTime() - new DateTime(2000, 1, 1)).TotalSeconds;
            double notification_time = current_time + /*distance * */ signal.modality.inverseTransmissionSpeed;

            Notification notification = new Notification();
            notification.time = notification_time;
            notification.sensor = sensor;
            notification.signal = signal;
            notificationQueue.Enqueue(notification);
        }

        SendSignals();
    }

    void SendSignals()
    {
        double current_time = (DateTime.Now.ToUniversalTime() - new DateTime(2000, 1, 1)).TotalSeconds;

        while(notificationQueue.Count > 0)
        {
            Notification notification = notificationQueue.Peek();
            
            if (notification.time < current_time)
            {
                switch(notification.sensor.agentType)
                {
                    case AgentTypes.Miner:
                        NotifyMiner(notification.signal);
                        notificationQueue.Dequeue();
                        break;
                    case AgentTypes.Outlaw:
                        NotifyOutlaw(notification.signal);
                        notificationQueue.Dequeue();
                        break;
                    case AgentTypes.Sheriff:
                        NotifySheriff(notification.signal);
                        notificationQueue.Dequeue();
                        break;
                    case AgentTypes.Undertaker:
                        NotifyUndertaker(notification.signal);
                        notificationQueue.Dequeue();
                        break;
                }
            }
            else
            {
                // Assuming queue is sorted
                break;
            }
            
        }
    }

    // Use this for initialization
    void Start () {
        controller = GameObject.Find("Controller");
        tilingSystem = controller.GetComponent<TilingSystem>();

        sensors = new List<Sensor>();
        notificationQueue = new Queue<Notification>();
    }
	
	// Update is called once per frame
	void Update () {
		/* 
        foreach Agent
        */
            
	}
}

public class Sensor
{
    public AgentTypes agentType;
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

    public Sensor(AgentTypes agentType, GameObject gameObject, List<SenseTypes> modalities, string agentName)
    {
        this.agentType = agentType;
        this.gameObject = gameObject;
        this.modalities = modalities;
        this.agentName = agentName;
    }
}

public class Signal
{
    public double signalStrength;
    public Sense modality;
    public Vector2 position;
}