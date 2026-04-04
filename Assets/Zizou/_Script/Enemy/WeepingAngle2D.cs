using UnityEngine;

public class WeepingAngele2D : MonoBehaviour
{
    [Header("References")]
    public Transform  player;
    public Flashlight flashlight;

    [Header("Movement")]
    public float moveSpeed   = 2f;
    public float attackRange = 0.5f;

    [Header("State")]
    public bool isFrozen = false;

    [Header("Visual Feedback")]
    public Color frozenColor = new Color(0.5f, 0.8f, 1f);
    public Color activeColor = Color.white;

    [Header("Sound")]
    public AudioClip movementSound; // drag your angel movement sound here

    private Rigidbody2D  rb;
    private SpriteRenderer sr;
    private AudioSource  audioSource;

    void Awake()
    {
        rb          = GetComponent<Rigidbody2D>();
        sr          = GetComponent<SpriteRenderer>();
        audioSource = GetComponent<AudioSource>();

        if (player == null)
        {
            GameObject p = GameObject.FindGameObjectWithTag("Player");
            if (p != null) player = p.transform;
        }

        if (flashlight == null)
            flashlight = FindFirstObjectByType<Flashlight>();

        // Set the movement sound clip if assigned
        if (audioSource != null && movementSound != null)
            audioSource.clip = movementSound;
    }

    void Update()
    {
        if (player == null || flashlight == null) return;

        isFrozen = flashlight.IsInCone(transform.position);

        UpdateVisual();
        HandleSound();
    }

    void FixedUpdate()
    {
        if (isFrozen) { rb.linearVelocity = Vector2.zero; return; }
        MoveTowardPlayer();
    }

    void MoveTowardPlayer()
    {
        if (player == null) return;
        Vector2 dir = ((Vector2)player.position - (Vector2)transform.position).normalized;
        rb.linearVelocity = dir * moveSpeed;
        if (sr != null) sr.flipX = dir.x < 0;
    }

    void UpdateVisual()
    {
        if (sr == null) return;
        sr.color = isFrozen ? frozenColor : activeColor;
    }

    void HandleSound()
    {
        if (audioSource == null) return;

        if (!isFrozen)
        {
            // Angel moving — play sound
            if (!audioSource.isPlaying)
                audioSource.Play();
        }
        else
        {
            // Angel frozen — stop sound
            if (audioSource.isPlaying)
                audioSource.Stop();
        }
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.CompareTag("Player")) OnReachPlayer();
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.CompareTag("Player")) OnReachPlayer();
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