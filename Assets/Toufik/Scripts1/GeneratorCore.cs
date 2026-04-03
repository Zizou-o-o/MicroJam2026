using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class GeneratorCore : MonoBehaviour
{
    [Header("Timers")]
    public float timeToFix = 30f;
    public float timeBetweenBreaks = 20f;
    private float currentTimer;

    [Header("Progression")]
    public int totalRepairsNeeded = 3;
    public int RepairsDone = 0;
    private bool isBroken = true;
    private bool hasWon = false;

    public TextMeshProUGUI machineStatusText;

    void Start()
    {
        currentTimer = timeToFix;
    }

    void Update()
    {
        if (hasWon) return;

        currentTimer -= Time.deltaTime;

        if (isBroken)
        {
            if (machineStatusText)
                machineStatusText.text = "CRITICAL FAILURE!\nTIME LEFT: " + Mathf.CeilToInt(currentTimer) + "s\nGOAL: " + RepairsDone + "/" + totalRepairsNeeded;

            if (currentTimer <= 0)
            {
                GameOver();
            }
        }
        else
        {
            if (machineStatusText)
                machineStatusText.text = "SYSTEM STABLE..\nBREAKING IN: " + Mathf.CeilToInt(currentTimer) + "s";

            if (currentTimer <= 0)
            {
                BreakAgain();
            }
        }
    }

    public void Repair(SamController sam)
    {
        if (isBroken && sam.hasVicePart)
        {
            sam.hasVicePart = false;
            RepairsDone++;

            if (RepairsDone >= totalRepairsNeeded)
            {
                WinGame();
            }
            else
            {
                isBroken = false;
                currentTimer = timeBetweenBreaks;
                if (machineStatusText) machineStatusText.text = "REPAIRED! STAY ALERT..";
            }
        }
    }

    void BreakAgain()
    {
        isBroken = true;
        currentTimer = timeToFix;
        Debug.Log("Generator has failed again");
    }

    void WinGame()
    {
        hasWon = true;

        if (machineStatusText) machineStatusText.text = "CORE STABILIZED! YOU WON!";

        // Stop the monster
        GameObject monster = GameObject.FindWithTag("Enemy");
        if (monster != null) monster.SetActive(false);

        // Return to hallway after 2 seconds
        Invoke(nameof(ReturnToHallway), 2f);
    }

    void GameOver()
    {
        Debug.Log("Game Over!");

        if (machineStatusText) machineStatusText.text = "SYSTEM FAILURE!\nGAME OVER";

        // Restart this minigame scene after 2 seconds
        Invoke(nameof(RestartScene), 2f);
    }

    void ReturnToHallway()
    {
        if (GameManager1.Instance != null)
            GameManager1.Instance.OnMinigameWon(); // go back to hallway
        else
            SceneManager.LoadScene(SceneManager.GetActiveScene().name); // fallback
    }

    void RestartScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}