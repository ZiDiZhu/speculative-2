using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//manages multiple oscillators
public class OscillatorManager : MonoBehaviour
{
    public List<Oscillator> myOscillatorList; //it's a list because I think it might be easier for adding/removing oscillators in game

    public int selectedOscIndex =0;

    //Shared UI for oscillators
    public Text OscillatorName;
    public Text modeText;
    public Text keyName;
    public Text waveName;

    public void ChangeWaveform()
    {
        myOscillatorList[selectedOscIndex].ChangeWaveform();
    }

    //get the index of next 
    public int LoopOscIndex()
    {
        int index = selectedOscIndex+1;
        if(myOscillatorList[selectedOscIndex] == null|| selectedOscIndex >= myOscillatorList.Count)
        {
            index = 0;
        }
        return index;
    }

    //quick Sync to the first oscillator 
    public void SyncAll()
    {
        float tempo = myOscillatorList[0].tempo;
        for (int i = 1; i < myOscillatorList.Count; i++)
        {
            if (myOscillatorList[i] != null)
            {
                myOscillatorList[i].tempo = tempo;
                myOscillatorList[i].timeNow = myOscillatorList[i - 1].timeNow;
            }

        }
    }

}
