using UnityEngine;

public class WeepingAngele2D : MonoBehaviour
{
    [Header("References")]
    public Transform player;        
    public Flashlight flashlight;  

    [Header("Movement")]
    public float moveSpeed = 2f;
    public float attackRange = 0.5f; // distance to kill

    [Header("State")]
    public bool isFrozen = false;   // public so you can see it in Inspector

    private Rigidbody2D rb;
    private SpriteRenderer sr;

    // Optional: visual feedback colors
    [Header("Visual Feedback")]
    public Color frozenColor = new Color(0.5f, 0.8f, 1f); 
    public Color activeColor = Color.white;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();

        // Auto-find if not assigned
        if (player == null)
        {
            GameObject p = GameObject.FindGameObjectWithTag("Player");
            if (p != null) player = p.transform;
        }

        if (flashlight == null)
            flashlight = FindFirstObjectByType<Flashlight>();
    }

    void Update()
    {
        if (player == null || flashlight == null) return;

        // Check if this angel is inside the flashlight cone
        isFrozen = flashlight.IsInCone(transform.position);

        UpdateVisual();
    }

    void FixedUpdate()
    {
        if (isFrozen)
        {
            rb.linearVelocity = Vector2.zero; // hard stop
            return;
        }

        MoveTowardPlayer();
    }

    void MoveTowardPlayer()
    {
        if (player == null) return;

        Vector2 dir = ((Vector2)player.position - (Vector2)transform.position).normalized;
        rb.linearVelocity = dir * moveSpeed;

        // Face the player (flip sprite)
        if (sr != null)
            sr.flipX = dir.x < 0;
    }

    void UpdateVisual()
    {
        if (sr == null) return;
        sr.color = isFrozen ? frozenColor : activeColor;
    }

//if the angl is close game over happens

    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.CompareTag("Player"))
        {
            OnReachPlayer();
        }
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.CompareTag("Player"))
        {
            OnReachPlayer();
        }
    }

    void OnReachPlayer()
    {
        FindFirstObjectByType<AngelMinigame>()?.OnPlayerCaught();
    }

    void OnDrawGizmos()
    {
        if (player == null) return;
        Gizmos.color = isFrozen ? Color.cyan : Color.red;
        Gizmos.DrawWireSphere(transform.position, 0.3f);
        Gizmos.DrawLine(transform.position, player.position);
    }
}
