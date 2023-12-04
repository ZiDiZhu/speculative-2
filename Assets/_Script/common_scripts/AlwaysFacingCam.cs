using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlwaysFacingCam : MonoBehaviour
{
    public Camera m_Camera;
    void LateUpdate()
    {
        if(m_Camera == null)
        {
            m_Camera=FindObjectOfType<CameraSwitch>().currentCam;

        }
        transform.LookAt(transform.position + m_Camera.transform.rotation * Vector3.forward,
            m_Camera.transform.rotation * Vector3.up);
    }
}
