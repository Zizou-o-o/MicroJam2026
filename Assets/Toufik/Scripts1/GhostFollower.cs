
using UnityEngine;

public class GhostFollower : MonoBehaviour
{
    [Header("References")]
    public Transform Player; 

    [Header("Normal Movement")]
    public float speed = 3f;

    [Header("Dash Attack")]
    public float dashTriggerDistance = 4f;  // how close before dash triggers
    public float dashSpeed = 12f; // how fast the dash is
    public float dashDuration = 0.3f;// how long the dash lasts
    public float dashCooldown = 3f;  // seconds before can dash again

    [Header("Dash Line Visual")]
    public LineRenderer dashLine;           // add a LineRenderer component and assign it
    public float lineDisplayTime = 0.15f;   // how long the line shows

    private Rigidbody2D rb;
    private GeneratorCore gn;

    // Dash state
    private bool isDashing = false;
    private float dashTimer = 0f;
    private float cooldownTimer = 0f;
    private Vector2 dashDirection = Vector2.zero;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        gn = GameObject.FindFirstObjectByType<GeneratorCore>();

        // Make sure line is hidden at start
        if (dashLine != null)
            dashLine.enabled = false;
    }

    void FixedUpdate()
    {
        // Stop if generator is fully repaired
        if (gn != null && gn.RepairsDone >= gn.totalRepairsNeeded)
        {
            rb.linearVelocity = Vector2.zero;
            return;
        }

        if (Player == null || rb == null) return;

        // Count down cooldown
        if (cooldownTimer > 0f)
            cooldownTimer -= Time.fixedDeltaTime;

        //Dashing
        if (isDashing)
        {
            dashTimer -= Time.fixedDeltaTime;
            rb.linearVelocity = dashDirection * dashSpeed;

            if (dashTimer <= 0f)
            {
                isDashing = false;
                cooldownTimer = dashCooldown;
            }
            return;
        }

        // Check if close enough to dash
        float distanceToPlayer = Vector2.Distance(transform.position, Player.position);

        if (distanceToPlayer <= dashTriggerDistance && cooldownTimer <= 0f)
        {
            StartDash();
            return;
        }

        //Normal follow movement
        Vector2 direction = ((Vector2)Player.position - (Vector2)transform.position).normalized;
        rb.linearVelocity = direction * speed;

        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        rb.SetRotation(angle);
    }

    void StartDash()
    {
        isDashing = true;
        dashTimer = dashDuration;
        dashDirection = ((Vector2)Player.position - (Vector2)transform.position).normalized;

        Debug.Log("[GhostFollower] Dashing!");

        // Show the dash line
        if (dashLine != null)
        {
            dashLine.enabled = true;
            dashLine.SetPosition(0, transform.position);
            dashLine.SetPosition(1, Player.position);
            Invoke(nameof(HideDashLine), lineDisplayTime);
        }
    }

    void HideDashLine()
    {
        if (dashLine != null)
            dashLine.enabled = false;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            // Stop dash on hit
            isDashing = false;
            rb.linearVelocity = Vector2.zero;

            FindFirstObjectByType<PlayerHealthVisual>()?.TakeDamage();
        }
    }
}