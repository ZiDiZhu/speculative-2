using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//A container of an array of child images indicating the combo level
public class ComboLightUI : MonoBehaviour
{
    public int startingLevel;
    public int numberOfLevels;
    public int currentLevel;
    public Image[] lights;

    // Start is called before the first frame update
    void Start()
    {
        numberOfLevels = lights.Length-1;
        SetLevel(startingLevel);
    }

    
    public void SetLevel(int level)
    {
        currentLevel = level;
        foreach (Image light in lights)
        {
            light.color = Color.gray;
        }
        lights[currentLevel].color = Color.white;
    }
    
    [ContextMenu("Reset")]
    public void Reset()
    {
        SetLevel(0);
    }
    
    [ContextMenu("Increase")]
    public void IncreaseLevel()
    {
        if (currentLevel < numberOfLevels)
        {
            SetLevel(currentLevel + 1);
        }
    }
    public void DecreaseLevel()
    {
        if (currentLevel > 0)
        {
            SetLevel(currentLevel - 1);
        }
    }

    public bool IsMaxLevel()
    {
        return currentLevel == numberOfLevels;
    }
    public void ResetLevel()
    {
        SetLevel(startingLevel);
    }
}
