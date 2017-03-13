using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SenseManager : MonoBehaviour {

    // Holds a record in the notification queue, ready to notify the sensor at the correct time.
    struct Notification<T>
    {
        int time;
        Agent<T> Sensor;
        string signal;
    }

    // Holds the list of sensors
    //Dictionary<int, Agent<T>> Sensors;

    // Holds a queue of notifcations waiting to be honoured
    Queue<Notification<Outlaw>> notificationQueue;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
