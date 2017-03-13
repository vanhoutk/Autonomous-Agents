using System;
using System.Collections.Generic;
using UnityEngine;

public class SenseManager : MonoBehaviour {

    // Holds a record in the notification queue, ready to notify the sensor at the correct time.
    struct Notification
    {
        public double time;
        public Sensor sensor;
        public Signal signal;
    }

    // Holds the list of sensors
    List<Sensor> sensors;

    // Holds a queue of notifcations waiting to be honoured
    Queue<Notification> notificationQueue;

    void addSignal(Signal signal)
    {
        // Aggregation Phase
        List<Sensor> valid_sensors = new List<Sensor>();
        foreach(Sensor sensor in sensors)
        {
            // Check if sensor detects the signal
            if (!sensor.detectsModality(signal.modality))
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

            // Perform additional modality specific checks
            // if(!signal.modality.extraChecks(signal, sensor)
            // continue;

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
                notification.sensor.notify(notification.signal);
                notificationQueue.Dequeue();
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
    public Vector2 position;

    public bool detectsModality(Sense modality)
    {
        return true;
    }

    public void notify(Signal signal)
    {

    }
}

public class Signal
{
    public double signalStrength;
    public Sense modality;
    public Vector2 position;
}