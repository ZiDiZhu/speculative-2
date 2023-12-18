using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DialogueUI : MonoBehaviour
{
    [SerializeField] private RectTransform dialogueBox;
    [SerializeField] private TextMeshProUGUI textLabel;

    public bool isOpen { get; set; }
    private ResponseHandler responseHandler;
    [SerializeField] private TypewriterEffect typewriterEffect;

    public bool canClose = false;


    // Start is called before the first frame update
    void Start()
    {
        if(responseHandler==null)responseHandler = GetComponent<ResponseHandler>();
        if(typewriterEffect==null)typewriterEffect = GetComponent<TypewriterEffect>();
    }

    public void CloseDialogueBox(){
        
        isOpen = false;
        dialogueBox.gameObject.SetActive(false);
        textLabel.text = "";
        
    }

    public void ShowDialogue(DialogueObject dialogueObject){
        dialogueBox.gameObject.SetActive(true);
        isOpen = true;
        StartCoroutine(StepThroughDialogue(dialogueObject));
        
    }
    
    private IEnumerator StepThroughDialogue(DialogueObject dialogueObject){
        for(int i = 0; i < dialogueObject.Dialogue.Length; i++){
            string dialogue = dialogueObject.Dialogue[i];
            yield return typewriterEffect.Run(dialogue, textLabel);
            if (i == dialogueObject.Dialogue.Length - 1 && dialogueObject.HasResponses) break;
            yield return new WaitUntil(() => Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.E));
        }
        if(dialogueObject.HasResponses){
            responseHandler.ShowResponses(dialogueObject.Responses);
        }
        else{
            dialogueBox.gameObject.SetActive(false);
            isOpen = false;
        }
    }


}
