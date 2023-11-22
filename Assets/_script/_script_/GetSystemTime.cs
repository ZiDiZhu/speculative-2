using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;

public class GetSystemTime : MonoBehaviour
{
    System.DateTime timeNow = System.DateTime.Now;
    public Text timeDisplay;
    public Text[] dates;

    private void Start()
    {
        //UpdateTime();
    }

    public void UpdateTime()
    {
        timeDisplay.text = timeNow.ToString("yyyy/MM/dd");
    }

    public void DateDisplay(int addDay)
    {
        for (int i = 0; i < dates.Length; i++)
        {
            dates[i].text = timeNow.Year + "/" + timeNow.Month + "/" + (timeNow.Day - i-1+addDay);
        }
    }
}
