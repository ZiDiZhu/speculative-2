using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public float timer =100f;
    public Text timerText;
    public bool timerOn;

    public int score = 0;
    public Text scoreText;

    public AudioSource audioSource;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (timerOn)
        {
            CountDown();
        }
        
    }

    void CountDown()
    {
        timer -= Time.deltaTime;
        int seconds = Mathf.FloorToInt(timer % 101); //remainder here is hard coded and must match the timer
        timerText.text = "timer: "+ seconds ;
        if(seconds <= 0)
        {
            timerOn = false;
        }
    }

    public void ScorePoint(int point)
    {
        score += point;
        scoreText.text = "Score: " + score;
    }

    public void PlaySfx(AudioClip clip)
    {
        audioSource.clip = clip;
        audioSource.Play();
    }
}
