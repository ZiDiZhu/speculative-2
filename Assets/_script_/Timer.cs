using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
    public int[] secondsToWait;
    public UnityEvent[] eventToTrigger;
    public string[] filmScript;
    public string[] filmScript2;
    public Text caption;
    public Text caption2;

    public Button btn1;
    // Start is called before the first frame update
    void Start()
    {
        StartCounting();
    }


    public void StartCounting()
    {
        for (int i = 0; i < eventToTrigger.Length; i++)
        {
            StartCoroutine(ExampleCoroutine(i));
            Cursor.lockState=CursorLockMode.None;
        }
    }

    IEnumerator ExampleCoroutine(int i)
    {
        yield return new WaitForSeconds(secondsToWait[i]);

        eventToTrigger[i].Invoke();

        caption.text = filmScript[i];
        caption2.text = filmScript2[i];
    }
}

