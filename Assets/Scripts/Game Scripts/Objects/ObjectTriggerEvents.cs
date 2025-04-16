using System;
using UnityEngine;
using UnityEngine.Events;

public class ObjectTriggerEvents : Events
{
    [Header("The Other GameObject to interact with")]
    public GameObject target;
    
    [Header("The Event that will run")]
    public UnityEvent onTriggerWithTarget;

    
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            
            target = other.gameObject;
            onTriggerWithTarget?.Invoke();
        }
    }
}
