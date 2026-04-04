using UnityEngine;

public class VoiceManager : MonoBehaviour
{
    public static VoiceManager instance;
    [Header("Audio Sources")]
    public AudioSource voiceSource;
    public AudioSource hallAmbience;



    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        if(instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);

        } else
        {
            Destroy(gameObject);
        }

        
    }

    //teleport sound

    public void PlayerTeleport()
    {
        if(voiceSource != null)
        {
            voiceSource.Stop();
    
            voiceSource.Play();


        }
    }

    // Update is called once per frame
    public void SetHallwayAmbience(bool active)
    {
        if(hallAmbience == null)return;
        if(active && !hallAmbience.isPlaying)
        {
            hallAmbience.Play();

        } else if (!active)
        {
            hallAmbience.Stop();
        }
    }
}
