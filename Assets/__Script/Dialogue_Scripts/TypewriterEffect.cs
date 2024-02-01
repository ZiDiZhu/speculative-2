using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.Audio;

//this code was taken from Semag Games' tutorial

public class TypewriterEffect : MonoBehaviour
{
    //note: SerializeField is used when you want to see a private  variable in the editor
    [SerializeField] private float typeSpeed = 50f;
    public AudioSource typeSound;
    public bool isPlayingSound = false;
    public bool isRunnnig = false;
    private Coroutine typeTextCoroutine;

    //polymorphism
    private TMP_Text textLabel;
    private Text textlabel;

    private string textToType;

    public Coroutine Run(string textToType, TMP_Text textLabel)
    {
        typeTextCoroutine = StartCoroutine(TypeText(textToType, textLabel));
        return typeTextCoroutine;
    }

    public Coroutine Run(string textToType, Text textLabel)
    {
        typeTextCoroutine = StartCoroutine(TypeText(textToType, textLabel));
        return typeTextCoroutine;
    }

    private void Start()
    {
        if (typeSound == null)
        {
            typeSound = GetComponent<AudioSource>();
        }
    }


    private void Update()
    {
        if(isRunnnig){
            if (Input.GetKeyDown(KeyCode.E) || Input.GetMouseButtonDown(0)|| Input.GetKeyDown(KeyCode.Space))
            {
                //Debug.Log("Skip Typing");
                if(textLabel!=null)SkipTyping(textLabel,textToType);
                if(textlabel!=null)SkipTyping(textlabel,textToType);
            }
        }
        
    }

    public void StopTyping()
    {
        if (typeTextCoroutine != null)
        {
            //Debug.Log("Stop Typing");
            //StopCoroutine(typeTextCoroutine);
            isRunnnig = false;
            if (typeSound != null)
            {
                typeSound.Stop();
                isPlayingSound = false;
            }
        }
    }

    private IEnumerator TypeText(string text, TMP_Text label)
    {
        textToType = text;
        textLabel = label;
        textLabel.text = string.Empty;
        isRunnnig = true;
        float t = 0;
        int charIndex = 0;

        while ((charIndex < textToType.Length)&&isRunnnig)
        {
            PlayTypeSound();
            t += Time.deltaTime * typeSpeed;
            charIndex = Mathf.FloorToInt(t);
            charIndex = Mathf.Clamp(charIndex, 0, textToType.Length);

            textLabel.text = textToType.Substring(0, charIndex);

            yield return null;
        }
        SkipTyping(textLabel, text);
    }

    private IEnumerator TypeText(string text, Text label)
    {
        textToType = text;
        textlabel = label;
        textlabel.text = string.Empty;
        isRunnnig = true;
        float t = 0;
        int charIndex = 0;

        while ((charIndex < textToType.Length) && isRunnnig)
        {
            PlayTypeSound();
            t += Time.deltaTime * typeSpeed;
            charIndex = Mathf.FloorToInt(t);
            charIndex = Mathf.Clamp(charIndex, 0, textToType.Length);

            textlabel.text = textToType.Substring(0, charIndex);

            yield return null;
        }
        SkipTyping(textlabel, text);
    }

    void PlayTypeSound()
    {
        if (!isPlayingSound && typeSound != null)
        {
            typeSound.Play();
            isPlayingSound = true;
        }
    }

    public void SkipTyping(TMP_Text textLabel, string textToType)
    {
        StopTyping();
        textLabel.text = textToType;
    }

    public void SkipTyping(Text textLabel, string textToType)
    {
        StopTyping();
        textLabel.text = textToType;
    }
}