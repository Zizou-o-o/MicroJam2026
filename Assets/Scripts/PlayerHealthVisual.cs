using UnityEngine;

public class PlayerHealthVisual : MonoBehaviour
{
    public GameObject[] hearts;
    private int currentHealth;
    private float lastHitTime;
    public float invincibilityDuration = 1f;

    void Start()
    {
        currentHealth = hearts.Length;
        Time.timeScale = 1f; // make sure timescale is reset when scene loads
    }

    public void TakeDamage()
    {
        if (Time.time < lastHitTime + invincibilityDuration) return;

        if (currentHealth > 0)
        {
            currentHealth--;
            hearts[currentHealth].SetActive(false);
            lastHitTime = Time.time;
            Debug.Log("Heart lost, remaining: " + currentHealth);

            if (currentHealth <= 0)
                Die();
        }

        StartCoroutine(CameraEffects.instance.Shake(0.2f, 0.15f));
    }

    void Die()
    {
        Debug.Log("Sam is lost in the loop!");
        Time.timeScale = 1f; // always reset timescale before loading a scene!
        Invoke(nameof(RestartScene), 1.5f);
    }

    void RestartScene()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(
            UnityEngine.SceneManagement.SceneManager.GetActiveScene().name);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            TakeDamage();
            Destroy(other.gameObject);
        }
    }
}
