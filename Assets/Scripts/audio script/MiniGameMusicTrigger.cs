using UnityEngine;

public class MiniGameMusicTrigger : MonoBehaviour
{
    public AudioClip thisGamesMusic; // Drag the specific song for THIS puzzle here

    void Start()
    {
        if (VoiceManager.instance != null)
        {
            // Turn off Hallway music, Turn on this Puzzle’s music
            VoiceManager.instance.SetHallwayAmbience(false);
            VoiceManager.instance.SetMiniGameMusic(thisGamesMusic, true);
        }
    }

    void OnDestroy()
    {
        if (VoiceManager.instance != null)
        {
            // Stop the puzzle music when we leave
            VoiceManager.instance.SetMiniGameMusic(null, false);
        }
    }
}

