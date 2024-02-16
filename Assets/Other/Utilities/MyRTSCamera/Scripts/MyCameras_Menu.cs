using UnityEngine;
using UnityEditor;

public class MyCameras_Menu
{
	//[MenuItem("My Cameras/My RTS Camera")]
    public static void GetOrbitalCamera()
	{
		GameObject cameraHolder = new GameObject("RTSCamera", typeof(RTSCamera));
		RTSCamera orbitalCam = cameraHolder.GetComponent<RTSCamera>();
		GameObject camera = new GameObject("Camera", typeof(Camera), typeof(AudioListener));
		camera.tag = "MainCamera";
		camera.transform.SetParent(cameraHolder.transform);
		camera.transform.localPosition = new Vector3(0f, 5f, -5f);
		camera.transform.localEulerAngles = new Vector3(45f, 0f, 0f);
		orbitalCam.orbitalCam = camera;
		orbitalCam.SetDefault();
	}
}
