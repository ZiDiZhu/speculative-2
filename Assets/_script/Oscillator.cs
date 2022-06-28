using UnityEngine;
using UnityEngine.UI;



// Attempting to make an oscillator based procedural music generator
// Search "To Do" to see what to do next

//To Do: clean up codes, make it clean and modular 

public class Oscillator : MonoBehaviour
{
    //robot 
    public GameObject robot;
    public Color[] skinColor;
    public Material skinMat;


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

    public int currentMode = 0;

    //in-scene console
    public Text consoleText;
    

    //Adjustable Stats 
    public float volume;
    public float tempo = 5f; // "bpm" 

    public string currentKey = "C"; //offset names
    public int currentNoteOffset; //index of which key it is on 
    public int currentKeyIndex;
    public string[] key = { "C", "D", "E", "F", "G" };

    public string currentScale = "major"; 
    public string[] scale = { "major", "minor"};
    public int scaleIndex = 0;

    public string waveForm = "sin"; //square, saw, tri
    public string[] waveform = { "sin", "square", "tri" };
    public int currentwaveformIndex = 0;

    public bool isPlaying = true;
    public bool isHolding = true;

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

    //holds the notes in a musical key
    [SerializeField] private float[] scaleNotes; // 0 = empty

    private void Start()
    {
        Initiate();
    }

    public void Initiate()
    {
        robot = gameObject.transform.GetChild(0).gameObject;
        InitializeFrequencies();
        gain = volume;
        robot.GetComponent<Animator>().enabled = true;
        robot.GetComponent<Animator>().speed = tempo; //dance to the tempo
        FindScale("C");
    }

    private void Update()
    {

        if (isPlaying) //it doesn't stop volume
        {
            Play(currentMode);
        }
    }

    //general purpose
    //loops through an array and get the next index 
    public int NextItemIndexInArray(int currentIndex, string[] a)
    {
        int index = currentIndex + 1;
        if (index >= a.Length)
        {
            index = 0;
        }
        return index;
    }

    public void ChangeWaveform()
    {
        currentwaveformIndex = NextItemIndexInArray(currentwaveformIndex, waveform);
        waveForm = waveform[currentwaveformIndex];
    }


    // to fix
    public void ChangeRhythm(int length) // length of ts array[0] 
    {
        currentRhythmIndex++;
        if (currentRhythmIndex + 1 >= length)
        {
            currentRhythmIndex = 0;
        }
    }


    public void ChangeKey()
    {
        currentKeyIndex = NextItemIndexInArray(currentKeyIndex, key);
        currentKey = key[currentKeyIndex];
        FindScale(currentKey);
    }

    public void ChangeScale()
    {
        scaleIndex = NextItemIndexInArray(scaleIndex,scale);
        currentScale = scale[scaleIndex];
        FindScale(currentKey);
    }

    public void ChangeMode()
    {
        gain = volume; //prevent when switch mode when gain = 0
        currentMode++;
        if (currentMode >= 3)
            currentMode = 0;//temp

        switch (currentMode)
        {
            case 0:
                robot.GetComponent<Animator>().Play("Base Layer.TPose", 0, tempo);
                break;
            case 1:
                robot.GetComponent<Animator>().Play("Base Layer.Flair", 0, tempo);
                break;
            case 2:
                robot.GetComponent<Animator>().Play("Base Layer.Dance", 0, tempo);
                break;
        }
    }

    //experimental visuals
    public void ChangeSkinColor(int index)
    {
        skinMat.color = skinColor[index%4];
        skinMat.SetColor("_EmissionColor", skinMat.color);
    }

    public void Play(int mode)
    {
        switch (mode)
        {
            case 0:
                Metronome();
                break;
            case 1:
                LoopScale_Temp();
                break;
            case 2:
                PlayRandomNotes();
                break;
            default:
                break;
        }
    }

    //maybe this can use a stop-motion-like animation
    //also: make it DOOM doom doom doom (1 loud e less loud, indicating the time signature of 4/4)
    public void Metronome()
    {
        
        timeNow -= tempo * Time.deltaTime;
        if (timeNow <= 0)
        {
            frequency = scaleNotes[0]; //arbitrary frequency
            noteDuration = 0.5f;
            timeNow = noteDuration;
            if (gain == 0)
            {
                gain = volume;
            }
            else
            {
                gain = 0;
                ChangeSkinColor(Random.Range(0, 3));
            }
                
        }
    }


    public void TogglePlaying()
    {
        isHolding = !isHolding;

        if (!isHolding)
            robot.GetComponent<Animator>().speed = 0;
        else
            robot.GetComponent<Animator>().speed = tempo;
    }

    

    public void PlayRandomNotes()
    {
        timeNow -= tempo * Time.deltaTime;

        if (timeNow <= 0)
        {
            frequency = scaleNotes[Random.Range(0,scaleNotes.Length-1)];

            //visuals
            ChangeSkinColor(Random.Range(0, 3));


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
            if (scaleNotes[freqIndex] == 0 || freqIndex +1 >= scaleNotes.Length)
            {
                freqIndex = 0;
            }

            frequency = scaleNotes[freqIndex];
            freqIndex++;

            

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

    // select the notes in the musical scale by formula
    public void FindScale(string scalename) 
    {
        switch (scalename)
        {
            case "C":
                currentNoteOffset = 0;
                break;
            case "D":
                currentNoteOffset = 2;
                break;
            case "E":
                currentNoteOffset = 4;
                break;
            case "F":
                currentNoteOffset = 5;
                break;
            case "G":
                currentNoteOffset = 7;
                break;
            default:
                Debug.Log("Invalid Key");
                break;
        }
        //assuming the scale has 8 notes + itself on higher octave
        //otherwise pentatone (5) +1 or blues scale(7) +1
        scaleNotes = new float[9];
        switch (currentScale)
        {
            case "major":
                for (int i = 0; i < scaleNotes.Length; i++)
                {
                    scaleNotes[i] = frequencies[i * 2 + currentNoteOffset]; //Major scale
                }
                break;

            case "minor": //natural minor
                scaleNotes[0] = frequencies[0 + currentNoteOffset];
                scaleNotes[1] = frequencies[2 + currentNoteOffset];
                scaleNotes[2] = frequencies[3 + currentNoteOffset];
                scaleNotes[3] = frequencies[6 + currentNoteOffset];
                scaleNotes[4] = frequencies[8 + currentNoteOffset];
                scaleNotes[5] = frequencies[9 + currentNoteOffset];
                scaleNotes[6] = frequencies[11 + currentNoteOffset];
                scaleNotes[7] = frequencies[14 + currentNoteOffset];
                break;

            default:
                Debug.Log("Invalid Scale");
                break;

        }

    }

    //this make sound!
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
                        data[i] = (float)gain * 0.6f;
                    }
                    else
                    {
                        data[i] = -(float)gain * 0.6f;
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
