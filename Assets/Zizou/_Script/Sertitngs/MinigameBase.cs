using UnityEngine;
//this i dont understand it much i will just leave it as this
/// <summary>
/// BASE CLASS — every minigame script must extend this.
///
/// ══════════════════════════════════════════════════
///  FOR TEAMMATES — how to use this:
/// ══════════════════════════════════════════════════
///
///  1. Create a new script, extend MinigameBase:
///       public class YourMinigame : MinigameBase { }
///
///  2. Override Setup() to initialize your minigame:
///       protected override void Setup(int hallway)
///       {
///           // hallway = 1, 2 or 3
///           // use this to scale your difficulty
///       }
///
///  3. When player WINS call:
///       CompleteMinigame();
///
///  4. When player LOSES call:
///       FailMinigame();   // restarts your scene
///
///  5. Tell the lead dev your exact scene name.
/// ══════════════════════════════════════════════════
/// </summary>
public abstract class MinigameBase : MonoBehaviour
{
    [Header("Transition Delays")]
    public float winDelay = 1.5f;
    public float loseDelay = 1.5f;

    protected int CurrentHallway => GameManager1.Instance != null ? GameManager1.Instance.CurrentHallway : 1;
    void Start()
    {
        Debug.Log($"[{GetType().Name}] Starting — Hallway {CurrentHallway}");
        Setup(CurrentHallway);
    }

    // Override this in your minigame
    protected abstract void Setup(int hallway);

    // Call this when player wins
    protected void CompleteMinigame()
    {
        Debug.Log($"[{GetType().Name}] Player WON!");
        Invoke(nameof(ReturnToHallway), winDelay);
    }

    // Call this when player loses
    protected void FailMinigame()
    {
        Debug.Log($"[{GetType().Name}] Player LOST — restarting...");
        Invoke(nameof(RestartScene), loseDelay);
    }

    void ReturnToHallway() => GameManager1.Instance?.OnMinigameWon();

    void RestartScene() =>
        UnityEngine.SceneManagement.SceneManager.LoadScene(
            UnityEngine.SceneManagement.SceneManager.GetActiveScene().name);
}
