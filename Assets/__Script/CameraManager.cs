using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    [SerializeField]private List<Camera> cameras;
    [SerializeField]private Camera activeCamera;
    
    public Camera GetActiveCamera(){
        return activeCamera;
    }

    public void SetCameraActive(Camera camera){
        foreach(Camera c in cameras){
            c.enabled = false;
        }
        camera.enabled = true;
        activeCamera = camera;
    }
}
