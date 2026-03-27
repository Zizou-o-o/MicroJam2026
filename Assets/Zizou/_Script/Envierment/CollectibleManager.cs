using UnityEngine;
using TMPro; // Remove this line if you're using regular UI Text instead of TextMeshPro

public class CollectibleManager : MonoBehaviour
{
    public static CollectibleManager Instance { get; private set; }
    [Header("Collectible")]
    public GameObject collectiblePrefab;   

    [Header("Spawn Area ")]
    public float spawnAreaWidth = 20f;     
    public float spawnAreaHeight = 20f;     

    [Header("Score UI")]
    public TextMeshProUGUI scoreText;    
                            
    [Header("Score")]
    public int totalCollectibles = 10;      

    private int score = 0;
    private GameObject currentCollectible; 

    void Awake()
    {
        if (Instance != null && Instance != this) { Destroy(gameObject); return; }
        Instance = this;
    }

    void Start()
    {
        UpdateScoreUI();
        SpawnCollectible();
    }

    public void OnCollected(Collectible col)
    {
        score++;
        UpdateScoreUI();

        Destroy(col.gameObject);
        currentCollectible = null;

        // Check if they got full number
        if (score >= totalCollectibles)
        {
            OnAllCollected();
            return;
        }
        //else it just countinue spawing
        SpawnCollectible();
    }

    void SpawnCollectible()
    {
        if (collectiblePrefab == null)
        {
            //for debuging
            Debug.LogWarning("CollectibleManager: No prefab assigned!");
            return;
        }

        Vector2 spawnPos = GetRandomSpawnPosition();
        currentCollectible = Instantiate(collectiblePrefab, spawnPos, Quaternion.identity);
    }

    Vector2 GetRandomSpawnPosition()
    {
        float centerX = transform.position.x;
        float centerY = transform.position.y;

        float x = Random.Range(centerX - spawnAreaWidth / 2f, centerX + spawnAreaWidth / 2f);
        float y = Random.Range(centerY - spawnAreaHeight / 2f, centerY + spawnAreaHeight / 2f);

        return new Vector2(x, y);
    }
    void UpdateScoreUI()
    {
        if (scoreText != null)
            scoreText.text = "Score: " + score;
        else
            Debug.LogWarning("CollectibleManager: scoreText not assigned in Inspector.");
    }
    void OnAllCollected()
    {
        Debug.Log("All collectibles gathered! You win!");

        //add in here where it take the player to after winning
    }

    //toshow the spawing area
    void OnDrawGizmos()
    {
        Gizmos.color = new Color(1f, 1f, 0f, 0.3f);
        Gizmos.DrawCube(transform.position, new Vector3(spawnAreaWidth, spawnAreaHeight, 0f));

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(transform.position, new Vector3(spawnAreaWidth, spawnAreaHeight, 0f));
    }
}