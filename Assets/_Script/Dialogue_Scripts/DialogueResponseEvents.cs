using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueResponseEvents : MonoBehaviour
{
    [SerializeField] private DialogueObject dialogueObject;
    [SerializeField] private ResponseEvent[] events;


    public ResponseEvent[] Events => events;

    // Update is called once per frame
    void Update()
    {
        
    }
}
