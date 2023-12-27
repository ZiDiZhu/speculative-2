using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//is a gameObject has a dialogue which can be activated by the player
public class DialogueActivator : MonoBehaviour
{

        
    public DialogueObject dialogueObject;
    public InputPromptDisplay inputPromptDisplay;   
    private DialogueUI dialogueUI;
    private Outline outline;
    // Start is called before the first frame update
    void Start()
    {
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
            dialogueUI.dialogueObject = dialogueObject;
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
            dialogueUI.dialogueObject = null;
            dialogueUI.CloseDialogueBox();
            if (inputPromptDisplay != null)
            {
                inputPromptDisplay.interact.SetActive(false);
            }
            
        }
    }
    
}
