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
    public Text selectedOscText;
    public Text modeText;
    public Text keyText;
    public Text waveText;
    public Text scaleText;


    public void ChangeWaveform()
    {
        myOscillatorList[selectedOscIndex].ChangeWaveform();
        UpdateUI(myOscillatorList[selectedOscIndex]);
    }

    public void ChangeKey()
    {
        myOscillatorList[selectedOscIndex].ChangeKey();
        UpdateUI(myOscillatorList[selectedOscIndex]);
    }

    public void ChangeScale()
    {
        myOscillatorList[selectedOscIndex].ChangeScale();
        UpdateUI(myOscillatorList[selectedOscIndex]);
    }

    public void ChangeMode()
    {
        myOscillatorList[selectedOscIndex].ChangeMode();
        UpdateUI(myOscillatorList[selectedOscIndex]);
    }

    //go to next oscillator
    public void SelectNextOsc()
    {
        myOscillatorList[selectedOscIndex].robot.GetComponent<Outline>().enabled = false; //turn off outline of last item

        int index = selectedOscIndex+1;
        if(index >= myOscillatorList.Count)
        {
            index = 0;
        }
        selectedOscIndex = index;
        UpdateUI(myOscillatorList[selectedOscIndex]);

        myOscillatorList[selectedOscIndex].robot.GetComponent<Outline>().enabled = true;

    }

    public void UpdateUI(Oscillator osc)
    {
        selectedOscText.text = "Oscillator " + selectedOscIndex;

        waveText.text = osc.waveForm+" wave";
        modeText.text = "Mode "+ osc.currentMode;
        keyText.text = osc.currentKey+" Key";
        scaleText.text = osc.currentScale;
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
