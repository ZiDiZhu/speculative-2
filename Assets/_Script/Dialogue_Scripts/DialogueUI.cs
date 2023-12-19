using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DialogueUI : MonoBehaviour
{
    [SerializeField] private RectTransform dialogueBox;
    [SerializeField] private TextMeshProUGUI textLabel;

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

    public void CloseDialogueBox(){
        responseHandler.CloseResponseBox();
        isOpen = false;
        dialogueBox.gameObject.SetActive(false);
        textLabel.text = "";
    }

    public void ShowDialogue(DialogueObject dialogueObject){
        if (isOpen&&!isNewDialogue) return;
        dialogueBox.gameObject.SetActive(true);
        isOpen = true;
        StartCoroutine(StepThroughDialogue(dialogueObject));
    }
    
    private IEnumerator StepThroughDialogue(DialogueObject dialogueObject){
        for(int i = 0; i < dialogueObject.Dialogue.Length; i++){
            string dialogue = dialogueObject.Dialogue[i];
            yield return typewriterEffect.Run(dialogue, textLabel);
            if (i == dialogueObject.Dialogue.Length - 1 && dialogueObject.HasResponses) break;
            yield return new WaitWhile(() => typewriterEffect.isRunnnig); 
            Debug.Log("Done Typing, Waiting for input");
            yield return new WaitUntil(() => Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.E));
            
        }
        if(dialogueObject.HasResponses){
            responseHandler.ShowResponses(dialogueObject.Responses);
        }
        else{
            CloseDialogueBox();
        }
    }


}
