using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class KeyEventPair
{
    public KeyCode key; // The key to trigger the event
    public UnityEvent response; // The event to invoke when the key is pressed
}

public class KeyToEventMapper : MonoBehaviour
{
    public KeyEventPair[] mappings;

    void Update()
    {
        foreach (var mapping in mappings)
        {
            if (Input.GetKeyDown(mapping.key))
            {
                mapping.response.Invoke();
            }
        }
    }
}
