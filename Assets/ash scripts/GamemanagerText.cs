using UnityEngine;

public class Gamemanager : MonoBehaviour
{
    public static Gamemanager Instance;
    public GameObject player;
    public GameObject[] enemies;

    void Awake()
    {
        Instance = this;
        player.SetActive(false);
        foreach (GameObject enemy in enemies)
            enemy.SetActive(false);
    }

    public void StartGame()
    {
        player.SetActive(true);
        foreach (GameObject enemy in enemies)
            enemy.SetActive(true);
    }
}