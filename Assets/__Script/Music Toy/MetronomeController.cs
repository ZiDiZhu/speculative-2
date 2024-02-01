using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MetronomeController : MonoBehaviour
{
    public Metronome metronome;
    public GameObject pendulum;
    public Transform pivotPoint; // Reference to the pivot point GameObject

    public float bpm = 140.0f; // Beats per minute for the metronome

    private float pendulumLength;
    private float angle = 0.0f;
    private float angularSpeed;
    void Start()
    {
        // Calculate the angular speed based on the BPM
        angularSpeed = 1.0f * Mathf.PI * bpm / 60.0f;
        if(metronome==null){
            metronome = FindObjectOfType<Metronome>();
        }
    }

    void Update()
    {
        // Calculate the pendulum's position
        angle += angularSpeed * Time.deltaTime;
        float xPos = Mathf.Sin(angle) * 2.0f; // Adjust the multiplier for the pendulum's length

        // Update the pendulum's position
        pendulumLength = Vector3.Distance(pendulum.transform.position, pivotPoint.position);
        angle = Mathf.Sin(Time.time * angularSpeed) * (pendulumLength * 45.0f);

        // Set the pendulum's rotation around the pivot point
        pendulum.transform.rotation = Quaternion.Euler(0.0f, 0.0f, angle);
    }

    public void RefreshBPM(){
        bpm = (float)metronome.bpm;
        angularSpeed = 1.0f * Mathf.PI * bpm / 60.0f;
    }

}
