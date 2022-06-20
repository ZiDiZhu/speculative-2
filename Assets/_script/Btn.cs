using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class Btn : MonoBehaviour
{
    private Button button;

    public string id;
    public string[] letter;

    private void Awake()
    {
        button = GetComponent<Button>();
        gameObject.GetComponentInChildren<Text>().text = id;
    }

    void Start()
    {
        button.onClick.AddListener(TaskOnClick);
        button.onClick.AddListener(delegate { TaskWithParameters("Hello"); });
        button.onClick.AddListener(() => ButtonClicked(42));
        button.onClick.AddListener(TaskOnClick);
    }


    void TaskOnClick()
    {
        //Output this to console when Button1 or Button3 is clicked
        Debug.Log("You have clicked the button!");
    }

    void TaskWithParameters(string message)
    {
        //Output this to console when the Button2 is clicked
        Debug.Log(message);
    }

    void ButtonClicked(int buttonNo)
    {
        //Output this to console when the Button3 is clicked
        Debug.Log("Button clicked = " + buttonNo);
    }
}
