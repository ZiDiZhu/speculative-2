using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class DialogueUI : MonoBehaviour
{
    [SerializeField] private Image speakerImage;
    [SerializeField] private RectTransform dialogueBox;
    [SerializeField] private Text textLabel;
    

    public DialogueObject dialogueObject;

    public bool isNewDialogue = true; //fix for the dialogue box not showing up on the first dialogue after a response

    public bool isOpen { get; set; }
    private ResponseHandler responseHandler;
    [SerializeField] private TypewriterEffect typewriterEffect;



    // Start is called before the first frame update
    void Start()
    {
        if(responseHandler==null)responseHandler = GetComponent<ResponseHandler>();
        if(typewriterEffect==null)typewriterEffect = GetComponent<TypewriterEffect>();
        //responseHandler.AddResponseEvent();
        if (dialogueObject != null)
        { //start with a dialogue if assigned one in the inspector
            ShowDialogue(dialogueObject);
        }else{
            CloseDialogueBox();
        }
    }

    // Update is called once per frame  
    void Update()
    {
        if(dialogueObject!=null&&!typewriterEffect.isRunnnig){
            if (Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.E))
            {
                ShowDialogue(dialogueObject);
            }
        }
    }
    
    //sets the dialogue object and its responses from an in-scene dialogue activator
    public void UpdateFromDialogueActicator(DialogueActivator da){
        this.dialogueObject = da.dialogueObject;
        responseHandler.SetResponseEvent(da.responeEvents);
    }

    public void ClearDialogueObject(){
        dialogueObject = null;
        responseHandler.ClearResponseEvents();
    }

    public void CloseDialogueBox(){
        responseHandler.CloseResponseBox();
        isOpen = false;
        dialogueBox.gameObject.SetActive(false);
        textLabel.text = "";
    }

    public void ShowDialogue(DialogueObject dialogueObject){
        if (isOpen||!isNewDialogue) return;
        isNewDialogue = false;
        dialogueBox.gameObject.SetActive(true);
        isOpen = true;
        speakerImage.sprite = dialogueObject.SpeakerSprite;
        StartCoroutine(StepThroughDialogue(dialogueObject));
    }
    
    private IEnumerator StepThroughDialogue(DialogueObject dialogueObject){
        for(int i = 0; i < dialogueObject.Dialogue.Length; i++){
            string dialogue = dialogueObject.SpeakerName+": \n"+ dialogueObject.Dialogue[i];
            yield return typewriterEffect.Run(dialogue, textLabel);
            if (i == dialogueObject.Dialogue.Length - 1 && dialogueObject.HasResponses) break;
            yield return new WaitWhile(() => typewriterEffect.isRunnnig); 
            //Debug.Log("Done Typing, Waiting for input");
            yield return new WaitUntil(() => Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.E)||Input.GetKeyDown(KeyCode.Space));
            
        }
        if(dialogueObject.HasResponses){
            responseHandler.ShowResponses(dialogueObject.Responses);
        }
        else{
            CloseDialogueBox();
        }
    }


}
