using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TypeText : MonoBehaviour
{

    //note: SerializeField is used when you want to see a private  variable in the editor
    [SerializeField] private float typeSpeed = 50f;
    public AudioSource typeSound;
    public bool isPlayingSound = false;

    [TextArea(15, 20)]
    public string txt;
    public Text label;
    public void Type()
    {
        Run(txt, label);
    }

    public void ChangeText(string newtext)
    {
        txt = newtext;
    }

    public Coroutine Run(string textToType, Text textLabel)
    {
        return StartCoroutine(Typetext(textToType, textLabel));
    }

    public IEnumerator Typetext(string textToType, Text textLabel)
    {

        textLabel.text = string.Empty;

        //yield return new WaitForSeconds(1);

        float t = 0;
        int charIndex = 0;

        while (charIndex < textToType.Length)
        {
            PlayTypeSound();
            t += Time.deltaTime * typeSpeed;
            charIndex = Mathf.FloorToInt(t);
            charIndex = Mathf.Clamp(charIndex, 0, textToType.Length);

            textLabel.text = textToType.Substring(0, charIndex);

            yield return null;
        }

        if (typeSound != null)
        {
            typeSound?.Stop();
        }
        isPlayingSound = false;
        textLabel.text = textToType;
    }

    public void PlayTypeSound()
    {
        if (!isPlayingSound && typeSound != null)
        {
            typeSound.Play();
            isPlayingSound = true;
        }
    }
}
