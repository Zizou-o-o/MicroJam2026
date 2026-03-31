
using UnityEngine;
using UnityEngine.SceneManagement;

public class DeathTouch : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("The ghost caught Sam!");
            Invoke(nameof(RestartScene), 1.5f);
        }
    }

    void RestartScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}