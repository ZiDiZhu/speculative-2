using UnityEngine;

public class TimeControl : MonoBehaviour
{
    public float slowDownFactor = 0.5f; // This is the factor by which time will be slowed down.

    void Update()
    {
        if(Time.timeScale != 0.0f){
            if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
            {
                Time.timeScale = slowDownFactor;
                Time.fixedDeltaTime = 0.02f * Time.timeScale; // Adjust fixedDeltaTime to maintain smooth physics.
            }
            else if(Input.GetKey(KeyCode.LeftControl))
            {
                Time.timeScale = 2.0f;
                Time.fixedDeltaTime = 0.02f * Time.timeScale; // Adjust fixedDeltaTime to maintain smooth physics.
            }else{
                Time.timeScale = 1.0f;
                Time.fixedDeltaTime = 0.02f * Time.timeScale; // Adjust fixedDeltaTime to maintain smooth physics.
            }
        }
        
    }
}

