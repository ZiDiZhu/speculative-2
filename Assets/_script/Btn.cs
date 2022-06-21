using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

//this script should be attached to each button of the dialpad
//child(0) text = id; child(1) text = letters (values set in UI)

public class Btn : MonoBehaviour
{

    public string id;
    public char[]letter;

    public string rawLetters;

    private void Awake()
    {
        id = gameObject.transform.GetChild(0).GetComponent<Text>().text;
        gameObject.name = id;

        rawLetters = gameObject.transform.GetChild(1).GetComponent<Text>().text;
    }

    void Start()
    {
        //assign letters to button
        rawLetters = rawLetters.Replace(" ", string.Empty);
        letter = rawLetters.ToCharArray();
    }


}
