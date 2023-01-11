using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;



//general sript for interaction when player is within range
public class Interactable : MonoBehaviour
{
    public string objectName;
    public Text objectNameText;
    public AudioClip sfx;
    public AudioSource audioSource;
    public GameObject inRangeObject; //sets active when in range
    //public GameManager gm;

    private void Start()
    {
        audioSource = gameObject.AddComponent<AudioSource>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            Debug.Log("Player walks to " + objectName);
            if(objectNameText)
                objectNameText.text = objectName;
            inRangeObject.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            Debug.Log("Player walks to " + objectName);
            if (objectNameText)
                objectNameText.text = "";
            inRangeObject.SetActive(false);
            audioSource.Stop();
        }
    }

    public void PlaySfx()
    {
        audioSource.clip = sfx;
        audioSource.Play();
    }


}
