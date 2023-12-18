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

    private DialogueUI dialogueUI;
    private List<GameObject> tempResponseButtons = new List<GameObject>();

    // Start is called before the first frame update
    void Start()
    {
        
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

            responseBoxHeight += responseButtonTemplate.sizeDelta.y;
        }

        responseBox.sizeDelta = new Vector2(responseBox.sizeDelta.x, responseBoxHeight);
        responseBox.gameObject.SetActive(true);
    }

    //when a response is clicked
    private void OnPickedResponse(Response response, int responseIndex)
    {

        Debug.Log("Picked Response");

        //close the response box
        responseBox.gameObject.SetActive(false);


        //destroy the response buttons
        foreach (GameObject button in tempResponseButtons)
        {
            Destroy(button);
        }

        tempResponseButtons.Clear();

        if (responseEvents != null && responseIndex <= responseEvents.Length)
        {
            responseEvents[responseIndex].OnPickedResponse?.Invoke();
        }

        responseEvents = null;


        if (response.NextDialogue)
        {
            Debug.Log("warning:response dialogue");
            //dialogueUI.CloseDialogueBox();
            //dialogueUI.ShowDialogue(response.DialogueObject);
        }
        else
        {
            Debug.Log("no response dialogue");
            
            dialogueUI.CloseDialogueBox();
            dialogueUI.canClose = true;
        }

    }
}
