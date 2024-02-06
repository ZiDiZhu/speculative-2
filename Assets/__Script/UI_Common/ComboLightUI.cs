using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//A container of an array of child images indicating the combo level
public class ComboLightUI : MonoBehaviour
{
    public int numberOfLevels;
    public int currentLevel;
    public Image[] lights;
    public Sprite lightOff;
    public Sprite[] lightOn; //one for each level
    public Color[] lightColor; //one for each level

    // Start is called before the first frame update
    void Start()
    {
        SetLevel(0);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetLevel(int level)
    {
        currentLevel = level;
        for (int i = 0; i < numberOfLevels; i++)
        {
            if (i < level)
            {
                lights[i].sprite = lightOn[i];
                lights[i].color = lightColor[i];
            }
            else
            {
                lights[i].sprite = lightOff;
            }
        }
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
}
