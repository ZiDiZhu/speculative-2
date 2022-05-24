using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;



// Attempting to make a music generator

public class Oscillator : MonoBehaviour
{
    public double frequency = 440.0;
    private double increment;
    private double phase;
    private double sampling_frequency = 48000.0;

    public float gain;  //for "input"
    public float volume = 0.1f; //for "output"

    public float[] frequencies; //stores musical Notes!
    public int freqIndex;

    public float tempo = 5f;
    public float sequenceTime = 1f;
    public float timeNow = 1f;

    public Text consoleText;
    public Text scaleName;


    public string currentKey = "C"; //offset names
    public string currentScale = "major"; //minor, pentaMajor, pentaMinor, blues  
    public int currentNoteOffset; //index of which key it is on

    //holds the notes in a musical scale
    [SerializeField] private float[] scaleNotes;

    private void Start()
    {

        InitializeFrequencies();
        FindScale(0); //C by default
    }

    //assign numbers to notes
    // 2 octaves of 12 equal temperament
    private void InitializeFrequencies()
    {
        frequencies = new float[24];
        //C4-B4
        frequencies[0] = 262; //C4
        frequencies[1] = 277; //CS4
        frequencies[2] = 294; //D4
        frequencies[3] = 311; //DS4
        frequencies[4] = 330; //E4
        frequencies[5] = 349; //F4
        frequencies[6] = 370; //FS4
        frequencies[7] = 392; //G4
        frequencies[8] = 415; //GS4
        frequencies[9] = 440; //A4
        frequencies[10] = 466; //AS4
        frequencies[11] = 494; //B4

        //C5-B5
        frequencies[12] = 523; //C5
        frequencies[13] = 554; //CS5
        frequencies[14] = 587; //D5
        frequencies[15] = 622; //DS5
        frequencies[16] = 659; //E5
        frequencies[17] = 698; //F5
        frequencies[18] = 740; //FS5
        frequencies[19] = 784; //G5
        frequencies[20] = 831; //GS5
        frequencies[21] = 880; //A5
        frequencies[22] = 932; //AS5
        frequencies[23] = 988; //B5

        gain = volume;
    }

    
    private void Update()
    {

        
        //basic beat 
        timeNow -= tempo * Time.deltaTime;
        if (timeNow <= 0)
        {
            frequency = scaleNotes[freqIndex];
            freqIndex++;
            consoleText.text =freqIndex+": "+ frequency+"Hz";

            if (freqIndex >= scaleNotes.Length - 1)
            {
                freqIndex = 0;
            }

            timeNow = sequenceTime;

        }

    }

    private void Beat()
    {
        //intermittent
        if (timeNow <= 0)
        {

            timeNow = sequenceTime;
            if (gain == volume)
            {
                gain = 0;
            }
            else if (gain != volume)
            {
                gain = volume;

                frequency = scaleNotes[freqIndex];
                freqIndex++;

                if (freqIndex >= scaleNotes.Length - 1)
                {
                    freqIndex = 0;
                }
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

    public void ChangeScale(string newScale)
    {
        currentScale = newScale;
        scaleName.text = currentKey +" " + currentScale;
        FindScale(currentNoteOffset);
    }

    // select the notes in the musical scale by formula
    public void FindScale(int offset) //offset is for index of starting note (C4 =0, E4= 6, etc)
    {
        currentNoteOffset = offset;
        switch (currentNoteOffset)
        {
            case 0:
                currentKey = "C";
                break;
            case 2:
                currentKey = "D";
                break;
            case 4:
                currentKey = "E";
                break;
            case 5:
                currentKey = "F";
                break;
            case 7:
                currentKey = "G";
                break;
        }
        scaleName.text = currentKey+ " " + currentScale;
        //assuming the scale has 8 notes + itself on higher octave
        //otherwise pentatone (5) +1 or blues scale(7) +1
        scaleNotes = new float[9];

        switch (currentScale)
        {
            case "major":
                for (int i = 0; i < scaleNotes.Length; i++)
                {
                    scaleNotes[i] = frequencies[i * 2 + offset]; //Major scale
                }
                break;
            case "minor": //natural minor
                scaleNotes[0] = frequencies[0 + offset];
                scaleNotes[1] = frequencies[2 + offset];
                scaleNotes[2] = frequencies[3 + offset];
                scaleNotes[3] = frequencies[6 + offset];
                scaleNotes[4] = frequencies[8 + offset];
                scaleNotes[5] = frequencies[9 + offset];
                scaleNotes[6] = frequencies[11 + offset];
                scaleNotes[7] = frequencies[14 + offset];
                break;

        }

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
