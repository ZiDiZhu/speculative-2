using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(AudioSource))]
public class NotePlayer : MonoBehaviour
{
    public bool isPlaying = false;

    public double frequency = 440; // Currently Playing
    private double increment;
    private double phase;
    private double sampling_frequency = 48000.0;
    public string waveForm = "sin"; //square, saw, tri
    public float gain;  //"raw" volume
    public float noteDuration = 0.5f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }



    private System.Collections.IEnumerator SustainForSeconds(float duration)
    {
        yield return new WaitForSeconds(duration);
        stopPlaying();
    }

    public void stopPlaying(){
        isPlaying = false;
    }

    private void OnAudioFilterRead(float[] data, int channels)
    {
        if (isPlaying)
        {
            increment = frequency * 2.0 * Mathf.PI / sampling_frequency;
            for (int i = 0; i < data.Length; i += channels)
            {
                phase += increment;
                if (waveForm == "sin")//sin wave
                {
                    data[i] = (float)(gain * Mathf.Sin((float)phase));
                }
                else if (waveForm == "square")//square wave
                {
                    if (gain * Mathf.Sin((float)phase) >= 0)
                    {
                        data[i] = (float)gain * 0.2f;
                    }
                    else
                    {
                        data[i] = -(float)gain * 0.2f;
                    }
                }
                else if (waveForm == "tri")//triangle wave
                {
                    data[i] = (float)(gain * (double)Mathf.PingPong((float)phase, 1.0f));
                }
                else
                {
                    Debug.Log("Invalid waveForm");
                }


                if (channels == 2)//stereo
                { data[i + 1] = data[i]; }

                if (phase > (Mathf.PI * 2))
                { phase = 0.0; }
            }
        }
    }
}
