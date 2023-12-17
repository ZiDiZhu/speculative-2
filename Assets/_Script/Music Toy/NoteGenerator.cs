using UnityEngine;
using UnityEditor;

public class NoteGenerator : MonoBehaviour
{
    public string noteNamePrefix = "C"; // Prefix for note names (e.g., "C", "D", "E", etc.)
    public int numOctaves = 8; // Number of octaves to generate
    public float duration = 1.0f;
    //public AudioClip sound;
    public float baseFrequency = 16.35f; // Frequency of C0

    public NoteData noteDataScriptableObj;

    [ContextMenu("Generate NoteData")]
    public void GenerateNoteData()
    {

        string folderPath = "Assets/NoteData"; // Path to the folder where NoteData assets should be stored
        if (!AssetDatabase.IsValidFolder(folderPath))
        {
            // Create the folder if it doesn't exist
            AssetDatabase.CreateFolder("Assets", "NoteData");
        }
        for (int octave = 0; octave <= numOctaves; octave++)
        {
            for (int noteIndex = 0; noteIndex < 12; noteIndex++) // 12 notes in an octave
            {
                // Create a new NoteData instance
                NoteData noteData = Instantiate(noteDataScriptableObj);

                // Generate note name based on note index and octave
                noteData.name = GetNoteName(noteIndex, octave);

                // Calculate the frequency based on the note's position relative to A4
                int semitoneOffset = GetSemitoneOffset(noteIndex);
                noteData.frequency = baseFrequency * Mathf.Pow(2.0f, (float)(semitoneOffset+(octave * 12) )/ 12.0f);
                noteData.octave = octave;

                // Assign the attributes from the Inspector
                noteData.duration = duration;
                //noteData.sound = sound;
                

                // Save the NoteData asset
                string assetPath = "Assets/NoteData/" + noteData.name + ".asset";
                AssetDatabase.CreateAsset(noteData, assetPath);
                AssetDatabase.SaveAssets();

                Debug.Log("NoteData created: " + noteData.name);
            }
        }
    }

    // Calculate the semitone offset relative to A4 for a given note index
    private int GetSemitoneOffset(int noteIndex)
    {
        // Define the semitone offsets for each note in an octave
        int[] semitoneOffsets = { 9, 11, 0, 2, 4, 5, 7 };

        // Calculate the semitone offset based on the note index
        return semitoneOffsets[noteIndex % 7];
    }

    // Helper method to generate note name with octave prefix
    private string GetNoteName(int noteIndex, int octave)
    {
        string[] noteNames = { "C", "C#", "D", "D#", "E", "F", "F#", "G", "G#", "A", "A#", "B" };
        return noteNames[noteIndex] + (octave + 1); // Octave number starts from 1
    }
}