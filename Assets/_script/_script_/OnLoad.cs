using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class OnLoad : MonoBehaviour //triggers 1 event on load after a time
{
    public float waitSeconds;
    public UnityEvent startEvent;
    void Start()
    {
        StartCoroutine(Countdown());

    }

    IEnumerator Countdown()
    {
        yield return new WaitForSeconds(waitSeconds);
        startEvent.Invoke();
    }
}
