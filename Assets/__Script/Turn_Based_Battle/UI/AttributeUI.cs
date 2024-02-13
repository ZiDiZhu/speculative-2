using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

//to be attached to the AttributeUI prefab, displays a slider for a given attribute
public class AttributeUI : MonoBehaviour
{
    [SerializeField]private TMP_Text text;
    [SerializeField] private Slider slider;


    public void SetAttribute(int value, int max)
    {
        slider.maxValue = max;
        slider.value = value;
    }

}
