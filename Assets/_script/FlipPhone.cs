using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class FlipPhone : MonoBehaviour
{
    public Btn[] btn;

    public InputField input;
    public string inputString = "";

    public string lastButtonId; //check if on the same btn
    public bool isTypingNew = true; //false = looping thru letters
    public int currentLetterIndex;

    void Start()
    {

        for (int i=0; i < btn.Length; i++)
        {
            int x = i;
            btn[i].GetComponent<Button>().onClick.AddListener(TaskOnClick);
            btn[i].GetComponent<Button>().onClick.AddListener(delegate { TaskWithParameters("Hello"); });

            btn[i].GetComponent<Button>().onClick.AddListener(() => ButtonClicked(x));
            Debug.Log(i);
        }
    }

    void TaskOnClick()
    {
        Debug.Log("You have clicked the button!");
    }

    void TaskWithParameters(string message)
    {
        //Output this to console when the Button2 is clicked
        Debug.Log(message);
    }


    void ButtonClicked(int index)
    {
        if(lastButtonId != btn[index].id)
        {
            isTypingNew = true;
            currentLetterIndex = 0;
        }
        else
        {
            isTypingNew = false;
        }

        lastButtonId = btn[index].id;

        if (!isTypingNew)
        {
            inputString=inputString.Remove(inputString.Length-1);
        }

        inputString += btn[index].letter[currentLetterIndex];
        currentLetterIndex++;
        if (currentLetterIndex >= btn[index].letter.Length)
        {
            currentLetterIndex = 0;
        }

        input.text = inputString;

    }

}

