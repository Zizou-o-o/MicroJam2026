using UnityEngine;


public class AngelMinigame : MinigameBase
{
    [Header("References")]
    public CollectibleManager collectibleManager;

    protected override void Setup(int hallway)
    {
        Debug.Log($"[AngelMinigame] Starting in Hallway {hallway}");
    }

    public void OnAllCollected()
    {
        CompleteMinigame(); // this returns the player to the hallway
    }

    public void OnPlayerCaught()
    {
        FailMinigame(); // this restarts the minigame scene
    }
}
