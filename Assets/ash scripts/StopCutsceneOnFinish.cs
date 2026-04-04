using UnityEngine;
using UnityEngine.Video;

public class StopCutsceneOnFinish : MonoBehaviour
{
    public VideoPlayer videoPlayer;

    void Start()
    {
        videoPlayer = GetComponent<VideoPlayer>();
        // Subscribe to event when video ends
        videoPlayer.loopPointReached += OnVideoFinished;
    }

    void OnVideoFinished(VideoPlayer vp)
    {
        // Disable the object
        gameObject.SetActive(false);

        // Optional: resume game if paused
        Time.timeScale = 1f;
    }
}
