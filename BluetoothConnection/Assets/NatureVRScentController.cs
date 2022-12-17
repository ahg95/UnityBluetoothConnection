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
    
    [Tooltip("Sources of the Lavender scent.")]
    public Collider[] LavenderColliders;

    [Tooltip("Sources of the Cedar scent.")]
    public Collider[] CedarColliders;
    
    [Range(0, 5)]
    [Tooltip("If the player is further away from a scent source than this value then they cannot smell it.")]
    public float ScentRadius = 1;
    
    [Range(0, 5)]
    [Tooltip("If the player is closer to a scent source than this value then they get the strongest smell.")]
    public float MaximumScentRadius = 0.25f;

    [Range(0, 180)]
    [Tooltip("The angle of the servo motor at which the scent source is closest to the player's nose.")]
    public float MinimumServoAngle = 0;
    
    [Range(0, 180)]
    [Tooltip("The angle of the servo motor at which the scent source is furthest from the player's nose.")]
    public float MaximumServoAngle = 180;
    
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
            // Lavender scent strength calculation
            
            Vector3 playerHeadPosition = PlayerHead.transform.position;
            
            // Minimum scent strength: 0
            // Maximum scent strength: 1
            float lavenderScentStrength = 0;
            float cedarScentStrength = 0;
            
            foreach (var collider in LavenderColliders)
            {
                Vector3 closestPoint = collider.ClosestPoint(playerHeadPosition);
                
                float scentDistance = Vector3.Distance(playerHeadPosition, closestPoint);

                lavenderScentStrength += Mathf.Max(0, (ScentRadius - scentDistance) / (ScentRadius - MaximumScentRadius));
            }
            
            foreach (var collider in CedarColliders)
            {
                Vector3 closestPoint = collider.ClosestPoint(playerHeadPosition);
                
                float scentDistance = Vector3.Distance(playerHeadPosition, closestPoint);
                
                cedarScentStrength += Mathf.Max(0, (ScentRadius - scentDistance) / (ScentRadius - MaximumScentRadius));
            }

            float lavenderServoAngle = Mathf.Lerp(MinimumServoAngle, MaximumServoAngle, lavenderScentStrength);
            float cedarServoAngle = Mathf.Lerp(MaximumServoAngle, MinimumServoAngle, cedarScentStrength);
            
            Debug.Log($"Setting lavender: {lavenderServoAngle}, cedar: {cedarServoAngle}");

            SerialController.SendSerialMessage($"{lavenderServoAngle},{cedarServoAngle}");

            yield return new WaitForSeconds(TimeBetweenUpdates);
        }
    }

    // Invoked when a line of data is received from the serial device.
    void OnMessageArrived(string msg)
    {
        Debug.Log($"Received serial message: {msg}");
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
