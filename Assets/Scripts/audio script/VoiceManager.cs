using UnityEngine;

public class VoiceManager : MonoBehaviour
{
    public static VoiceManager instance;
    [Header("Audio Sources")]
    public AudioSource voiceSource;
    public AudioSource hallAmbience;

    public AudioSource miniGameSource ;//dedicatedfor first mini game ghost room




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

    public void SetMiniGameMusic(AudioClip gameMusic , bool active)
    {
        if(miniGameSource == null) return;
        if(active && gameMusic != null)
        {
            miniGameSource.clip = gameMusic;
            miniGameSource.loop = true; 
            miniGameSource.Play();
        }
        else
        {
            miniGameSource.Stop();
        }



    }
}
