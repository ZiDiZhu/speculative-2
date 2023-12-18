using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ResponseHandler : MonoBehaviour
{
    [SerializeField] private RectTransform responseBox;
    [SerializeField] private RectTransform responseButtonTemplate;
    [SerializeField] private RectTransform responseContainer;

    [SerializeField] private ResponseEvent[] responseEvents;

    [SerializeField]private DialogueUI dialogueUI;
    private List<GameObject> tempResponseButtons = new List<GameObject>();

    // Start is called before the first frame update
    void Start()
    {
        dialogueUI = GetComponent<DialogueUI>();
    }

    public void AddResponseEvent(ResponseEvent[] responseEvents)
    {
        this.responseEvents = responseEvents;
    }

    public void ShowResponses(Response[] responses){
        float responseBoxHeight = 0;


        for (int i = 0; i < responses.Length; i++)
        {
            Response response = responses[i];
            int responseIndex = i;

            GameObject responseButton = Instantiate(responseButtonTemplate.gameObject, responseContainer);
            responseButton.gameObject.SetActive(true);
            responseButton.GetComponent<TMP_Text>().text = response.ResponseText;

            //turns response into button
            responseButton.GetComponent<Button>().onClick.AddListener(() => OnPickedResponse(response, responseIndex));

            tempResponseButtons.Add(responseButton);

        }

        responseBox.sizeDelta = new Vector2(responseBox.sizeDelta.x, responseBoxHeight);
        responseBox.gameObject.SetActive(true);
    }

    //when a response is clicked
    private void OnPickedResponse(Response response, int responseIndex)
    {

        Debug.Log("Picked Response");

        //destroy the response buttons
        foreach (GameObject button in tempResponseButtons)
        {
            Destroy(button);
        }

        //if (responseEvents != null && responseIndex <= responseEvents.Length)
        //{
        //    responseEvents[responseIndex].OnPickedResponse?.Invoke();
        //}
        

        //if a response has a following dialogue, show it
        if (response.NextDialogue!=null)
        {
            Debug.Log("response dialogue");
            dialogueUI.dialogueObject = response.NextDialogue;
            dialogueUI.ShowDialogue(dialogueUI.dialogueObject);
        }
        else
        {
            Debug.Log("no response dialogue");
            dialogueUI.CloseDialogueBox();
        }

        //close the response box
        tempResponseButtons.Clear();
        responseEvents = null;
        responseBox.gameObject.SetActive(false);

    }
}
