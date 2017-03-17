using System;
using System.Collections.Generic;
using UnityEngine;

// Holds a record in the notification queue, ready to notify the sensor at the correct time.
public struct Notification : IComparable<Notification>
{
    public double time;
    public Sensor sensor;
    public Signal signal;

    public int CompareTo(Notification other)
    {
        return time.CompareTo(other.time);
    }
}

public class SenseManager : MonoBehaviour
{
    // Variables
    private GameObject controller;
    //private Queue<Notification> notificationQueue; // Holds a queue of notifcations waiting to be honoured
    private List<Notification> notificationList;
    private TilingSystem tilingSystem;

    //public List<Sensor> sensors; // Holds the list of sensors
    public Dictionary<string, Sensor> sensors; // Holds the list of sensors

    // Notification Events
    public delegate void MinerNotify(Signal signal);
    public static event MinerNotify NotifyMiner;

    public delegate void OutlawNotify(Signal signal);
    public static event OutlawNotify NotifyOutlaw;

    public delegate void SheriffNotify(Signal signal);
    public static event SheriffNotify NotifySheriff;

    public delegate void UndertakerNotify(Signal signal);
    public static event UndertakerNotify NotifyUndertaker;

    // Functions
    /*
     * public void AddSignal(Signal signal)
     * private void SendSignals()
     * public void Start()
     * public void Update()
     */

    public void AddSignal(Signal signal)
    {
        // Aggregation Phase
        foreach(KeyValuePair<string, Sensor> sensor in sensors)
        {
            // Check if sensor detects the signal
            if (!sensor.Value.DetectsModality(signal.modality.senseType))
                continue;

            // Check how intense the sense is at the sensors location and whether it's below the sensor's threshold for experiencing it
            Vector3 current_position = sensor.Value.gameObject.transform.position;
            // TODO: Need to confirm this is correct
            Vector2 current_location = new Vector2(current_position.x/tilingSystem.tileSize + tilingSystem.CurrentPosition.x, current_position.y/tilingSystem.tileSize + tilingSystem.CurrentPosition.y);
            SquareGrid mapGrid = tilingSystem.mapGrid;
            // Path from signal to sensor
            AStarSearch sense_path = new AStarSearch(mapGrid, mapGrid.nodeSet[new Coordinates((int)signal.position.x, (int)signal.position.y)], mapGrid.nodeSet[new Coordinates((int)current_location.x, (int)current_location.y)], signal.modality.senseType);
            double path_cost = sense_path.gScore[mapGrid.nodeSet[new Coordinates((int)current_location.x, (int)current_location.y)]];
            double intensity = Math.Max(signal.signalStrength - path_cost, 0);
            double distance = EuclidianDistance(current_location, signal.position);

            // If the intensity at the sensor is less than the sensor's threshold for experiencing the sense
            if (intensity < sensor.Value.thresholds[signal.modality.senseType])
                continue;

            // Perform additional modality specific checks
            if(!signal.modality.extraChecks(signal, sensor.Value))
                continue;

            // Create the notification and send it
            double current_time = (DateTime.Now.ToUniversalTime() - new DateTime(2000, 1, 1)).TotalSeconds;
            double notification_time = current_time + distance * signal.modality.inverseTransmissionSpeed;

            Notification notification = new Notification();
            notification.time = notification_time;
            notification.sensor = sensor.Value;
            notification.signal = signal;
            //notificationQueue.Enqueue(notification);
            notificationList.Add(notification);
        }

        notificationList.Sort();
        SendSignals();
    }

    double EuclidianDistance(Vector2 a, Vector2 b)
    {
        return Math.Sqrt(Math.Pow((a.x - b.x), 2) + Math.Pow((a.y - b.y), 2));
    }

    void SendSignals()
    {
        double current_time = (DateTime.Now.ToUniversalTime() - new DateTime(2000, 1, 1)).TotalSeconds;

        while(notificationList.Count > 0)
        {
            //Notification notification = notificationList.Peek();
            Notification notification = notificationList[0];
            
            if (notification.time < current_time)
            {
                switch(notification.sensor.agentType)
                {
                    case AgentTypes.Miner:
                        NotifyMiner(notification.signal);
                        //notificationQueue.Dequeue();
                        notificationList.RemoveAt(0);
                        break;
                    case AgentTypes.Outlaw:
                        NotifyOutlaw(notification.signal);
                        //notificationQueue.Dequeue();
                        notificationList.RemoveAt(0);
                        break;
                    case AgentTypes.Sheriff:
                        NotifySheriff(notification.signal);
                        //notificationQueue.Dequeue();
                        notificationList.RemoveAt(0);
                        break;
                    case AgentTypes.Undertaker:
                        NotifyUndertaker(notification.signal);
                        //notificationQueue.Dequeue();
                        notificationList.RemoveAt(0);
                        break;
                }
            }
            else
            {
                break;
            }
            
        }
    }

    void Start ()
    {
        controller = GameObject.Find("Controller");
        tilingSystem = controller.GetComponent<TilingSystem>();

        sensors = new Dictionary<string, Sensor>();
        //notificationQueue = new Queue<Notification>();
        notificationList = new List<Notification>();
    }
	
	void Update ()
    {
        if (notificationList.Count > 0)
        {
            SendSignals();
        }
	}
}