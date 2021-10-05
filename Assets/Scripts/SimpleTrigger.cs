using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class SimpleTrigger : MonoBehaviour
{
    public string triggerTag = "Player";
    public bool disableAfterTriggering = true;
    public UnityEvent eventToTrigger;

    private bool triggered = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!triggered && other.CompareTag(triggerTag))
        {
            triggered = disableAfterTriggering;
            eventToTrigger.Invoke();
        }
    }
}
