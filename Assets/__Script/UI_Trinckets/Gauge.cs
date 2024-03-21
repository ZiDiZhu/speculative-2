using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gauge : MonoBehaviour
{

    [SerializeField] private const float max_angle = 20;
    [SerializeField] private const float min_angle = 160;
    [SerializeField] private Transform needleTransform;
    [SerializeField] private float min_value = 0;
    [SerializeField] private float max_value = 100;
    [SerializeField] private float current_value = 0;

    // Start is called before the first frame update
    void Start()
    {
        //SetValue(0);
        SetValue(current_value);
    }

    // Update is called once per frame
    void Update()
    {
        //SetValue(current_value);
    }


    public void SetValue(float value)
    {
        float angle = Mathf.Lerp(min_angle, max_angle, Mathf.InverseLerp(min_value, max_value, value));
        needleTransform.eulerAngles = new Vector3(0, 0, angle);
    }

    public void IncreaseValue(float value){
        current_value += value;
        if(current_value > max_value){
            current_value = max_value;
        }
        SetValue(current_value);
    }
}
