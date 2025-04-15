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
        target = target != null ? target : GameObject.FindGameObjectWithTag("Player");
        if (other.gameObject == target)
        {
            target = p;
            onTriggerWithTarget?.Invoke();
        }
    }



}
