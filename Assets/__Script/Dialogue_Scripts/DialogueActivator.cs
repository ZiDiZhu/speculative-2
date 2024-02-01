using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

// This script is used to activate a dialogue in scene.
// It is attached to a gameObject with a trigger collider, has a dialogue which can be activated by the player
// !!! IT ALSO has a list of response events to trigger when the dialogue is activated
// each response has a scene event index which is the index of the scene event to trigger
public class DialogueActivator : MonoBehaviour
{

        
    public DialogueObject dialogueObject; //the dialogue object to activate
    public List<UnityEvent> responeEvents; //the list of response events to trigger


    InputPromptDisplay inputPromptDisplay;   
    DialogueUI dialogueUI;
    
    Outline outline;

    // Start is called before the first frame update
    void Start()
    {
        //get component references
        outline = GetComponentInChildren<Outline>();
        dialogueUI = FindObjectOfType<DialogueUI>();
        if (outline != null) outline.enabled = false;
        if (inputPromptDisplay == null) inputPromptDisplay = FindObjectOfType<InputPromptDisplay>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (outline != null) outline.enabled = true;
            dialogueUI.UpdateFromDialogueActicator(this);
            
            if (inputPromptDisplay != null)
            {
                inputPromptDisplay.interact.SetActive(true);
            }
        }
    }
    
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (outline != null) outline.enabled = false;
            dialogueUI.ClearDialogueObject();
            dialogueUI.CloseDialogueBox();
            if (inputPromptDisplay != null)
            {
                inputPromptDisplay.interact.SetActive(false);
            }
            
        }
    }
    
}
