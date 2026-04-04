using UnityEngine;
using UnityEngine.SceneManagement; // Required for switching scenes

public class MainMenu : MonoBehaviour
{
    [Header("Scene Settings")]
    public string firstLevelName = "Hallway"; // The name of your first scene

    // 1. THE START BUTTON
    public void PlayGame()
    {
        // This loads the hallway scene
        SceneManager.LoadScene(firstLevelName);
    }

    // 2. THE QUIT BUTTON
    public void QuitGame()
    {
        Debug.Log("Player has quit the game.");
        Application.Quit(); // This works in the final built version of the game
    }
}
