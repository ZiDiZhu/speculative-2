using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class ResponseHandler : MonoBehaviour
{
    [SerializeField] private RectTransform responseBox;
    [SerializeField] private RectTransform responseButtonTemplate;
    [SerializeField] private RectTransform responseContainer;

    [SerializeField] private List<UnityEvent> responseEvents; //TODO: make this a reference to the dialogue object's response events

    [SerializeField]private DialogueUI dialogueUI;
    private List<GameObject> tempResponseButtons = new List<GameObject>();

    // Start is called before the first frame update
    void Start()
    {
        if(dialogueUI==null)dialogueUI = GetComponent<DialogueUI>();
        responseButtonTemplate.gameObject.SetActive(false);
    }

    // Sets the response events
    public void SetResponseEvent(List<UnityEvent> events)
    {
        this.responseEvents = events;
    }

    public void ClearResponseEvents(){
        responseEvents = null;
    }

    // Shows all selectable responses
    public void ShowResponses(Response[] responses){
        float responseBoxHeight = 0;


        for (int i = 0; i < responses.Length; i++)
        {
            Response response = responses[i];
            int responseIndex = 0; //TODO: make this variable link to r the response index ID

            GameObject responseButton = Instantiate(responseButtonTemplate.gameObject, responseContainer);
            responseButton.gameObject.SetActive(true);
            responseButton.GetComponent<TMP_Text>().text = response.ResponseText;

            //make the button click link to the event by its eventindex TODO
            responseButton.GetComponent<Button>().onClick.AddListener(() => OnPickedResponse(response, responseIndex));

            tempResponseButtons.Add(responseButton);

        }

        responseBox.sizeDelta = new Vector2(responseBox.sizeDelta.x, responseBoxHeight);
        responseBox.gameObject.SetActive(true);
    }

    //when a response is clicked
    //responseIndex is used to determine which response EVENT to trigger was clicked
    //it's this way because dialogues are scriptable objects and can't contain scene-specific events
    //this the responseIndex is used to determine which event to trigger from the scene
    //TODO: make this better
    private void OnPickedResponse(Response response, int responseIndex)
    {

        Debug.Log("Picked Response");
        dialogueUI.isNewDialogue = true;

        //trigger the response event
        if (responseEvents != null && responseIndex < responseEvents.Count)
        {
            Debug.Log("Triggering Response Event");
            responseEvents[responseIndex]?.Invoke();
        }


        if (response.NextDialogue==null){
            dialogueUI.CloseDialogueBox();
        }else{
            //Debug.Log("response dialogue");
            dialogueUI.dialogueObject = response.NextDialogue;
            dialogueUI.ShowDialogue(dialogueUI.dialogueObject);
        }
        CloseResponseBox();

    }

    public void CloseResponseBox()
    {
        foreach (GameObject button in tempResponseButtons)
        {
            Destroy(button);
        }
        tempResponseButtons.Clear();
        responseEvents = null;
        responseBox.gameObject.SetActive(false);
    }
}
