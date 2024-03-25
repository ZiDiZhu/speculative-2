using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class WebVideoPlayer : MonoBehaviour
{

    [SerializeField] string videoFileName;
    private VideoPlayer videoPlayer;

    public GameObject toEnableOnVideoEnd;

    // Start is called before the first frame update
    void Start()
    {
        videoPlayer = gameObject.GetComponent<VideoPlayer>();
        PlayVideo();
    }

    public void PlayVideo()
    {
        if (videoPlayer)
        {
            string videoPath = System.IO.Path.Combine(Application.streamingAssetsPath, videoFileName);
            videoPlayer.url = videoPath;
            videoPlayer.Play();
        }

    }

    void EndCutscene()
    {
        Destroy(gameObject);
        if (toEnableOnVideoEnd != null) toEnableOnVideoEnd.SetActive(true);
    }


}
