using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class MetronomeUI : MonoBehaviour
{
    public Metronome metronome; 

    public Text tempoText;

    private void Awake()
    {
        if(metronome==null){
            metronome = FindObjectOfType<Metronome>();
        }
    }

    public void SetTempoText(){
        tempoText.text = metronome.bpm.ToString();
    }

}
