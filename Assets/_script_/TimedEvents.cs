using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TimedEvents : MonoBehaviour
{
    //has to manually make sure both array have the same lengths
    public float[] timeStamp;
    public UnityEvent[] eventToTrigger;

    public void StartCounting()
    {
        for (int i = 0; i < eventToTrigger.Length; i++)
        {
            StartCoroutine(InvokeEvents(i));
        }
    }

    IEnumerator InvokeEvents(int i)
    {
        yield return new WaitForSeconds(timeStamp[i]);

        eventToTrigger[i].Invoke();

    }
}
