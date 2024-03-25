using UnityEngine;

//camera and lights too
public class CameraSwitch : MonoBehaviour
{
    public Camera cam1;
    public Camera cam2;
    public Camera cam3;
    public Camera playerCam;

    public Camera currentCam;

    public GameObject light1;
    public GameObject light2;

    private void Start()
    {
        PlayerCam();
    }
    public void Cam1()
    {
        SetActiveCamera(cam1);
    }

    public void Cam2()
    {
        SetActiveCamera(cam2);
    }

    public void Cam3()
    {
        SetActiveCamera(cam3);
    }

    public void PlayerCam(){
        SetActiveCamera(playerCam);
    }

    public void SetActiveCamera(Camera cam){
        cam1.enabled = false;
        cam2.enabled = false;
        cam3.enabled = false;
        playerCam.enabled = false;
        if (cam != null){
            cam.enabled = true;
            currentCam = cam;
        }
    }


    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            Cam1();
        }else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            Cam2();
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            Cam3();
        }else if(Input.GetKeyDown(KeyCode.P)){
            PlayerCam();
        }

        if (Input.GetKeyDown(KeyCode.Q))
        {
            if (light1.activeSelf == true)
                light1.SetActive(false);
            else
                light1.SetActive(true);
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            if (light2.activeSelf == true)
                light2.SetActive(false);
            else
                light2.SetActive(true);
        }
    }
}
