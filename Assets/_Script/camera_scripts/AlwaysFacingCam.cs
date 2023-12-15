using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlwaysFacingCam : MonoBehaviour
{
    public Camera m_Camera;

    private void Start()
    {
        if(m_Camera==null){
            m_Camera = Camera.current;
        }
    }
    void LateUpdate()
    {
        transform.LookAt(transform.position + m_Camera.transform.rotation * Vector3.forward,
            m_Camera.transform.rotation * Vector3.up);
    }
}
