using UnityEngine;

/// <summary>
/// Attach this to an empty GameObject in your minigame scene.
/// This is the bridge between your collectibles/angels and the GameManager.
///
/// SETUP:
///   1. Create empty GameObject in minigame scene → name it "AngelMinigame"
///   2. Attach this script
///   3. Assign the CollectibleManager from your scene in the Inspector
/// </summary>
public class AngelMinigame : MinigameBase
{
    [Header("References")]
    public CollectibleManager collectibleManager;

    protected override void Setup(int hallway)
    {
        // Nothing special needed here — collectibles and angels
        // are already placed in the scene by you
        Debug.Log($"[AngelMinigame] Starting in Hallway {hallway}");
    }

    // ── Called by CollectibleManager when all collectibles are picked up ──
    // You need to add ONE line to CollectibleManager.cs (see below)
    public void OnAllCollected()
    {
        CompleteMinigame(); // this returns the player to the hallway!
    }

    // ── Called by WeepingAngel when it catches the player ──
    // You need to add ONE line to WeepingAngel.cs (see below)
    public void OnPlayerCaught()
    {
        FailMinigame(); // this restarts the minigame scene
    }
}
