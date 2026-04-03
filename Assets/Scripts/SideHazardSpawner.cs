
using UnityEngine;
using System.Collections;

public class SideHazardSpawner : MonoBehaviour
{
    [Header("Sync with Main Timer")]
    public float timer = 60f;

    [Header("Projectile Settings")]
    public GameObject sideProjectilePrefab;
    public float spawnInterval = 4f;
    public float spawnHeight = -10.5f;
    public float projectilespeed = 18f;

    void Start()
    {
        StartCoroutine(SideAttackLoop());
    }

    void Update()
    {
        if (timer > 0)
            timer -= Time.deltaTime;
    }

    IEnumerator SideAttackLoop()
    {
        while (timer > 0)
        {
            yield return new WaitForSeconds(spawnInterval);
            float spawnX = (Random.value > 0.5f) ? -20f : 20f;
            bool fromLeft = (spawnX < 0);
            if (timer > 0)
                LaunchProjectile(spawnX, spawnHeight, fromLeft);
        }

        // Timer ran out — player survived! 
        Debug.Log("Survived! You win!");
        OnPlayerWin();
    }

    void LaunchProjectile(float x, float y, bool left)
    {
        GameObject projectile = Instantiate(sideProjectilePrefab, new Vector3(x, y, 0), Quaternion.identity);
        Rigidbody2D rb = projectile.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.gravityScale = 0;
            float direction = left ? 1 : -1;
            rb.linearVelocity = new Vector2(direction * projectilespeed, 0);
        }
        Destroy(projectile, 4f);
    }

    void OnPlayerWin()
    {
        // Return to hallway via GameManager
        if (GameManager1.Instance != null)
            GameManager1.Instance.OnMinigameWon();
        else
            UnityEngine.SceneManagement.SceneManager.LoadScene(
                UnityEngine.SceneManagement.SceneManager.GetActiveScene().name);
    }
}
