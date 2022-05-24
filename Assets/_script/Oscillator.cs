using System.Collections;
using System.Collections.Generic;
using UnityEngine;



// Attempting to make a basic Analog Synthesizer as a base for the music generator

public class Oscillator : MonoBehaviour
{
    public double frequency = 440.0;
    private double increment;
    private double phase;
    private double sampling_frequency = 48000.0;

    public float gain;  //for "input"
    public float volume = 0.1f; //for "output"

    public float[] frequencies;
    public int freqIndex;

    public float tempo = 5f;
    public float sequenceTime = 1f;
    public float timeNow = 1f;

    private void Start()
    {
    }


    private void Update()
    {
        //basic beat 
        timeNow -= tempo * Time.deltaTime;
        if (timeNow <= 0)
        {
            timeNow = sequenceTime;
            if(gain == volume)
            {
                gain = 0;
            }else if(gain != volume)
            {
                gain = volume;
            }
        }


    }

    private void BasicInput()
    {
        //testing: press space to make sound
        if (Input.GetKeyDown(KeyCode.Space))
        {
            gain = volume;
        }
        if (Input.GetKeyUp(KeyCode.Space))
        {
            gain = 0;
        }
    }

    // select the notes in the musical scale by formula
    public void FindScale()
    {

    }

    //this make sound!
    private void OnAudioFilterRead(float[] data, int channels)
    {
        increment = frequency * 2.0 * Mathf.PI / sampling_frequency;

        for (int i = 0; i < data.Length; i += channels)
        {
            phase += increment;
            data[i] = (float)(gain * Mathf.Sin((float)phase));

            //make sure playing in stereo 
            if (channels == 2)
            {
                data[i + 1] = data[i];
            }

            if (phase > (Mathf.PI * 2))
            {
                phase = 0.0;
            }
        }

    }

}
