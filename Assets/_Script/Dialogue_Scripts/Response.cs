using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class Response
{
    public string responseText; //the response text
    [SerializeField] private DialogueObject nextDialogue; //the next dialogue object

    [SerializeField] private bool isEndDialogue; //is this the end of the dialogue?
    public bool IsEndDialogue => isEndDialogue; //reference to the isEndDialogue bool

    public string ResponseText => responseText; //reference to the response text
    public DialogueObject NextDialogue => nextDialogue; //reference to the next dialogue object
}

[System.Serializable]
public class ResponseEvent
{
    [HideInInspector] public string eventname;
    [SerializeField] private UnityEvent onPickedResponse;

    public UnityEvent OnPickedResponse => onPickedResponse;
}