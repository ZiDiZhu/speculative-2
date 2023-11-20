using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Key{
    C,
    D,
    E,
    F,
    G,
    A,
    B
}

[CreateAssetMenu(fileName = "New Musical Note", menuName = "Musical Note")]
public class NoteData : ScriptableObject
{
    public Key key;
    public int octave;
    public float frequency;
}
