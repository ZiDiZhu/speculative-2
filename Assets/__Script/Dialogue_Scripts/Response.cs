using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class Response
{
    public string responseText; //the response text

    [SerializeField] private DialogueObject nextDialogue; //the next dialogue object
    public int sceneEventIndex; //the index of the scene event to trigger (see DialogueActivator -> responseEvents)

    public string ResponseText => responseText; //reference to the response text
    public DialogueObject NextDialogue => nextDialogue; //reference to the next dialogue object


}