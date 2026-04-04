using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager1 : MonoBehaviour
{
    public static GameManager1 Instance { get; private set; }

    [Header("Hallway Scenes")]
    public string hallway1Scene = "Hallway1";
    public string hallway2Scene = "Hallway2";
    public string hallway3Scene = "Hallway3";
    public string hallway4Scene = "Hallway4"; 

    [Header("Minigame Scenes — Hallway 1")]
    public string mg1_loop1 = "Minigame1";
    public string mg2_loop1 = "Minigame2";
    public string mg3_loop1 = "Minigame3";

    [Header("Minigame Scenes — Hallway 2")]
    public string mg1_loop2 = "Minigame1_Hard";
    public string mg2_loop2 = "Minigame2_Hard";
    public string mg3_loop2 = "Minigame3_Hard";

    [Header("Minigame Scenes — Hallway 3")]
    public string mg1_loop3 = "Minigame1_Hardest";
    public string mg2_loop3 = "Minigame2_Hardest";
    public string mg3_loop3 = "Minigame3_Hardest";

    [Header("Cutscene (teammate handles this)")]
    public string cutsceneScene = "Cutscene";

    //follow where the player is 
    [Header("Debug — Current State")]
    [SerializeField] private int currentHallway = 1; // 1, 2, 3 or 4
    [SerializeField] private int minigamesWonThisHallway = 0; // 0, 1, 2 or 3

    public int CurrentHallway => currentHallway;
    public int MinigamesWonThisHallway => minigamesWonThisHallway;

    void Awake()
    {
        if (Instance != null && Instance != this) { Destroy(gameObject); return; }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    //the trigger that take the player to the other minigames
    public void EnterMinigame(int minigameNumber)
    {
        //voice add
        if(VoiceManager.instance != null)
        {
            VoiceManager.instance.SetHallwayAmbience(false);
        }
        Debug.Log($"[GameManager] Entering Minigame {minigameNumber} | Hallway {currentHallway}");
        SceneManager.LoadScene(GetMinigameScene(currentHallway, minigameNumber));
    }

    string GetMinigameScene(int hallway, int minigame)
    {
        return hallway switch
        {
            1 => minigame switch { 1 => mg1_loop1, 2 => mg2_loop1, _ => mg3_loop1 },
            2 => minigame switch { 1 => mg1_loop2, 2 => mg2_loop2, _ => mg3_loop2 },
            3 => minigame switch { 1 => mg1_loop3, 2 => mg2_loop3, _ => mg3_loop3 },
            _ => mg1_loop1
        };
    }

    //check what minigame he finished
    public void OnMinigameWon()
    {

        //voice add teleport
        if(VoiceManager.instance!= null)
        {
            VoiceManager.instance.PlayerTeleport();

        }





        minigamesWonThisHallway++;
        Debug.Log($"[GameManager] Minigame won! ({minigamesWonThisHallway}/3 in Hallway {currentHallway})");
        LoadCurrentHallway();
    }

    // ── Called by HallwayEndTrigger ──────────────────────────────
    public void OnHallwayEnd()
    {
        // Hallway4 has no minigames — end goes straight to cutscene
        if (currentHallway == 4)
        {
            Debug.Log("[GameManager] End of Hallway4 — loading cutscene!");
            SceneManager.LoadScene(cutsceneScene);
            return;
        }

        // debug stuff
        if (minigamesWonThisHallway < 3)
        {
            Debug.Log("[GameManager] Can't proceed — finish all 3 minigames first!");
            return;
        }

        // All 3 minigames done , move to next hallway
        currentHallway++;
        minigamesWonThisHallway = 0;
        Debug.Log($"[GameManager] Moving to Hallway {currentHallway}!");
        LoadCurrentHallway();
    }

    void LoadCurrentHallway()
    {

        //voice addition
        if(VoiceManager.instance != null)
        {
            VoiceManager.instance.SetHallwayAmbience(true);
        }
        switch (currentHallway)
        {
            case 1: SceneManager.LoadScene(hallway1Scene); break;
            case 2: SceneManager.LoadScene(hallway2Scene); break;
            case 3: SceneManager.LoadScene(hallway3Scene); break;
            case 4: SceneManager.LoadScene(hallway4Scene); break;
        }
    }

    public void ResetGame()
    {
        currentHallway = 1;
        minigamesWonThisHallway = 0;
        SceneManager.LoadScene(hallway1Scene);
    }
}