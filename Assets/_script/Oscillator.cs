using UnityEngine;
using UnityEngine.UI;



// Attempting to make an oscillator based procedural music generator
// Search "To Do" to see what to do next

//To Do: clean up codes, make it clean and modular 

public class Oscillator : MonoBehaviour
{

    //Sound Generation "Raw" variales
    public double frequency = 440; // Currently Playing
    private double increment;
    private double phase;
    private double sampling_frequency = 48000.0;
    public float gain;  //"raw" volume
    public float noteDuration = 1f; //current note duration
    public int noteDurationNowIndex; // refers to the index in the array of ts 
    public float timeNow = 1f; //the timer of currently playing note
    public int freqIndex; //index of currently selected frequency
    public int currentNoteOffset; //index of which key it is on 

    //in-scene console
    public Text consoleText;
    public Text scaleName;

    //Adjustable Stats 
    public float volume;
    public float tempo = 5f; // "bpm" 
    public string currentKey = "C"; //offset names
    public string currentScale = "major"; //minor, pentaMajor, pentaMinor, blues  

    public int currentRhythmIndex = 0; // refers to ts array first index
    bool autoRhythmIsOn = false; //TOGGLE to automatically loop through ts array

    // here 1 = standard note; 0 = empty; -1= end  
    //static reference
    public float[] frequencies; //stores musical Notes!

    public float[,] ts3 =
        { {1,1,1,-1,0,0},
          {1,1,0.333f,0.333f,0.334f,0.333f}, 
          {4,-1,0,0,0,0},
          {2,1,0.5f,0.5f,-1,0},
          {0.5f,0.5f,0.5f,0.5f,0.5f,0.5f}};
    
    public float[,] ts4 =
        { {1,1,1,1,-1,0,0,0},
          {1,1,0.333f,0.333f,0.334f,0.333f,0.333f,0.334f}, // "fixed" 1/12 notes , this adds perfectly to 1 measure
          {4,-1,0,0,0,0,0,0},
          {2,1,0.5f,0.5f,-1,0,0,0},
          {0.5f,0.5f,0.5f,0.5f,0.5f,0.5f,0.5f,0.5f}}; 

    //holds the notes in a musical scale
    [SerializeField] private float[] scaleNotes; // 0 = empty

    private void Start()
    {
        InitializeFrequencies();
        gain = volume;
        FindScale(0); //C by default
    }

    private void Update()
    {
        //LoopScale_Temp();
        PlayRandomNotes();
    }

    public void Play()
    {

    }

    //change value with slider // make this into an enum (dropdown menu) later
    public void VolumeSlider(Slider slider)
    {
        volume = slider.value * 0.1f;
        gain = volume;
    }
    

    public void PlayRandomNotes()
    {
        timeNow -= tempo * Time.deltaTime;

        if (timeNow <= 0)
        {
            frequency = scaleNotes[Random.Range(0,scaleNotes.Length-1)];
            consoleText.text = freqIndex + ": " + frequency + "Hz";

            noteDuration = ts4[1, ++noteDurationNowIndex];

            if (noteDurationNowIndex + 1 >= ts4.GetLength(1))
            {
                noteDurationNowIndex = 0;
                if (autoRhythmIsOn) //loops thru the whole ts array
                {
                    ChangeRhythm(ts4.GetLength(0));
                }

            }
            timeNow = noteDuration;
        }
    }

    public void LoopScale_Temp()
    {
        timeNow -= tempo * Time.deltaTime;

        if (timeNow <= 0)
        {
            frequency = scaleNotes[freqIndex];
            freqIndex++;
            consoleText.text = freqIndex + ": " + frequency + "Hz";

            if (scaleNotes[freqIndex]==0)
            {
                freqIndex = 0;
            }

            noteDuration = ts4[1, ++noteDurationNowIndex];

            if (noteDurationNowIndex + 1 >= ts4.GetLength(1))
            {
                noteDurationNowIndex = 0;
                if (autoRhythmIsOn) //loops thru the whole ts array
                {
                    ChangeRhythm(ts4.GetLength(0));
                }

            }
            timeNow = noteDuration;
        }
    }

    
    //loop thru note duration patterns within the same ts group
    //rn its hard-coded to make it work on a button
    //to do: make it better? (not need to enter value in inspector/auto detects array length)
    public void ChangeRhythm(int length) // length of ts array[0] 
    {
        currentRhythmIndex++;
        if(currentRhythmIndex +1 >= length) 
        {
            currentRhythmIndex = 0;
        }
    }



    private void Beat()
    {
        //intermittent
        if (timeNow <= 0)
        {

            timeNow = noteDuration;
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
            default:
                Debug.Log("Invalid Key");
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

            default:
                Debug.Log("Invalid Scale");
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
            if (channels == 2)//stereo
            {data[i + 1] = data[i];}

            if (phase > (Mathf.PI * 2))
            {phase = 0.0;}
        }

    }

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
    }

}
