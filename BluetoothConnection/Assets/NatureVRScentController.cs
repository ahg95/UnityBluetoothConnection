using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class NatureVRScentController : MonoBehaviour
{
    public SerialController SerialController;

    [Tooltip("The head of the player, which will be used to calculate the distance to sources of scent.")]
    public Transform PlayerHead;
    
    [Tooltip("The sources of scent. The strongest scent is inside the collider.")]
    public Collider[] ScentSourceColliders;

    [Range(0, 5)]
    [Tooltip("Determines how quickly the strength of the scent falls of with distance. For example, a value of 1 means that at a distance of 1 there will be no scent.")]
    public float WeakestScentMinimumDistance = 1;

    [Range(0, 180)]
    [Tooltip("The angle of the servo motor at which the scent source is closest to the player's nose.")]
    public float ClosestServoAngle = 0;
    
    [Range(0, 180)]
    [Tooltip("The angle of the servo motor at which the scent source is furthest from the player's nose.")]
    public float FurthestServoAngle = 180;
    
    [Range(0.01f, 1)]
    [Tooltip("How much time should elapse before the next update of the servo motor.")]
    public float TimeBetweenUpdates = 0.2f;
    
    private void Start()
    {
        StartCoroutine(UpdateScentStrength());
    }

    IEnumerator UpdateScentStrength()
    {
        while (true)
        {
            float scentStrength = 0;

            Vector3 playerHeadPosition = PlayerHead.transform.position;
            
            foreach (var collider in ScentSourceColliders)
            {
                Vector3 closestPoint = collider.ClosestPoint(playerHeadPosition);
                
                float scentDistance = Vector3.Distance(playerHeadPosition, closestPoint);
                
                scentStrength += Mathf.Clamp((WeakestScentMinimumDistance - scentDistance) / WeakestScentMinimumDistance, 0, 1);
            }

            scentStrength = Mathf.Clamp(scentStrength, 0, 1);

            float servoAngle = Mathf.Lerp(FurthestServoAngle, ClosestServoAngle, scentStrength);

            Debug.Log("Set servo to angle " + servoAngle);
        
            SerialController.SendSerialMessage(servoAngle.ToString());
            
            yield return new WaitForSeconds(TimeBetweenUpdates);
        }
    }

    // Invoked when a line of data is received from the serial device.
    void OnMessageArrived(string msg)
    {
        Debug.Log("Micro bit received message: " + msg);
    }

    // Invoked when a connect/disconnect event occurs. The parameter 'success'
    // will be 'true' upon connection, and 'false' upon disconnection or
    // failure to connect.
    void OnConnectionEvent(bool success)
    {
        if (success)
            Debug.Log("Found the microbit!");
        else
            Debug.Log("Did not find the microbit.");
    }
}
