using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class KeyPressEventPair
{
    public KeyCode triggerKey;
    public UnityEvent onKeyPressed;
}

public class EventOnKeyPress : MonoBehaviour
{
    [Tooltip("A list of key-event pairs.")]
    public List<KeyPressEventPair> keyEventPairs = new List<KeyPressEventPair>();

    private void Update()
    {
        foreach (var keyEventPair in keyEventPairs)
        {
            if (Input.GetKeyDown(keyEventPair.triggerKey))
            {
                keyEventPair.onKeyPressed.Invoke();
            }
        }
    }
}
