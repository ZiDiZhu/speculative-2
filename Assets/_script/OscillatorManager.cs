using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//manages multiple oscillators
public class OscillatorManager : MonoBehaviour
{
    public GameObject oscPrefab;

    public List<Oscillator> myOscillatorList; //it's a list because I think it might be easier for adding/removing oscillators in game


    public int selectedOscIndex =0;
    public bool selected;

    //Shared UI for oscillators
    public Text selectedOscText;
    public Text modeText;
    public Text keyText;
    public Text waveText;
    public Text scaleText;
    public Text rangeText;

    public Slider volumeSlider,tempoSlider;

    public Color gray; //to show it's off

    public void CreateNewOsc()
    {
        GameObject temp;
        temp = Instantiate(oscPrefab, new Vector3(Random.Range(-10, 10), 0, Random.Range(-10, 10)),Quaternion.identity);
        myOscillatorList.Add(temp.GetComponent<Oscillator>());
        SelectNextOsc();//quick fix to get rid of outline 
        selectedOscIndex = myOscillatorList.Count - 1;//select the one just created
    }

    public void ToggleOnOff()
    {

        //flip on/off the audio
        myOscillatorList[selectedOscIndex].isPlaying = !myOscillatorList[selectedOscIndex].isPlaying;

        if (!myOscillatorList[selectedOscIndex].isPlaying)
        {
            myOscillatorList[selectedOscIndex].skinMat.SetColor("_EmissionColor", gray);
            myOscillatorList[selectedOscIndex].robot.GetComponent<Animator>().enabled = false;
        }
        else
        {
            myOscillatorList[selectedOscIndex].ChangeSkinColor(1);
            myOscillatorList[selectedOscIndex].robot.GetComponent<Animator>().enabled = true;
        }

    }

    public void Kill()
    {
        ToggleOnOff();
        StartCoroutine(Wait());
        
    }

    IEnumerator Wait()
    {
        yield return new WaitForSeconds(3);
        Destroy(myOscillatorList[selectedOscIndex].gameObject);
        myOscillatorList.RemoveAt(selectedOscIndex);
    }

    public void AddOsc(GameObject newOsc)
    {

    }

    public void ChangeRange()
    {
        myOscillatorList[selectedOscIndex].ChangeRange();
        UpdateUI(myOscillatorList[selectedOscIndex]);
    }

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

    public void VolumeSlider(Slider slider)
    {
        myOscillatorList[selectedOscIndex].volume = slider.value * 0.1f;
        myOscillatorList[selectedOscIndex].gain = myOscillatorList[selectedOscIndex].volume;
    }

    public void TempoSlider(Slider slider)
    {
        myOscillatorList[selectedOscIndex].tempo = slider.value;
        myOscillatorList[selectedOscIndex].robot.GetComponent<Animator>().speed = myOscillatorList[selectedOscIndex].tempo;
    }

    //go to next oscillator
    public void SelectNextOsc()
    {

        int index = selectedOscIndex+1;
        if(index >= myOscillatorList.Count)
        {
            index = 0;
        }
        selectedOscIndex = index;
        UpdateUI(myOscillatorList[selectedOscIndex]);

        for (int i=0; i <myOscillatorList.Count; i++)
        {
            myOscillatorList[i].robot.GetComponent<Outline>().enabled = false;
        } //turn off outline for everyone else

        for (int i = 0; i < myOscillatorList.Count; i++)
        {
            myOscillatorList[i].robot.GetComponent<Outline>().enabled = false;
        } //turn off outline for everyone else

        myOscillatorList[selectedOscIndex].robot.GetComponent<Outline>().enabled = true;

    }

    public void Select()
    {

        for (int i = 0; i < myOscillatorList.Count; i++)
        {
            if(myOscillatorList[i].hovering == true)
            {
                selectedOscIndex = i;
            }
        } 

        for (int i = 0; i < myOscillatorList.Count; i++)
        {
            myOscillatorList[i].robot.GetComponent<Outline>().enabled = false;
        } //turn off outline for everyone else

        myOscillatorList[selectedOscIndex].robot.GetComponent<Outline>().enabled = true;
        UpdateUI(myOscillatorList[selectedOscIndex]);
    }

    public void UpdateUI(Oscillator osc)
    {
        selectedOscText.text = "Oscillator " + selectedOscIndex;

        waveText.text = osc.waveForm+" wave";
        modeText.text = "Mode "+ osc.currentMode;
        keyText.text = osc.currentKey+" Key";
        scaleText.text = osc.currentScale;
        rangeText.text = osc.range;

        volumeSlider.value = myOscillatorList[selectedOscIndex].volume*10;
        tempoSlider.value = myOscillatorList[selectedOscIndex].tempo;
    }

    //quick Sync to the first oscillator 
    public void SyncAll()
    {
        float tempo = myOscillatorList[0].tempo;
        for (int i = 1; i < myOscillatorList.Count; i++)
        {
            if (myOscillatorList[i] != null)
            {
                myOscillatorList[i].tempo = tempo*0.5f;
                myOscillatorList[i].timeNow = myOscillatorList[i - 1].timeNow;
            }

        }
    }

}
