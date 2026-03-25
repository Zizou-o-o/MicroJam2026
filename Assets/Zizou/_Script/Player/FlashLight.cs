using UnityEngine;

public class Flashlight : MonoBehaviour
{
    [Header("References")]
    public PlayerMovement player; 

    [Header("Cone Settings")]
    [Range(10f, 120f)]
    public float coneAngle = 60f;       // full angle of the flashlight cone
    public float coneRange = 8f;        // how far the light reaches

    void Awake()
    {
        if (player == null)
            player = GetComponentInParent<PlayerMovement>();
    }

    void Update()
    {
        RotateToFacing();
    }

    void RotateToFacing()
    {
        Vector2 dir = player.facingDirection;
        if (dir == Vector2.zero) return;

        // Convert direction to angle
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0f, 0f, angle);
    }

  
    public bool IsInCone(Vector2 targetPosition)
    {
        Vector2 origin = (Vector2)transform.position;
        Vector2 toTarget = targetPosition - origin;

        // Distance check
        if (toTarget.magnitude > coneRange) return false;

        // Angle check
        float angleToTarget = Vector2.Angle(player.facingDirection, toTarget);
        return angleToTarget <= coneAngle / 2f;
    }

    // ---------------------------------------------------------------
    // Optional: Draw the cone in the Scene view for easy debugging
    // ---------------------------------------------------------------
    void OnDrawGizmosSelected()
    {
        if (player == null) return;

        Vector2 origin = transform.position;
        Vector2 dir = player.facingDirection == Vector2.zero ? Vector2.right : player.facingDirection;

        float halfAngle = coneAngle / 2f;
        float baseAngle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;

        Vector2 leftEdge = AngleToDir(baseAngle - halfAngle) * coneRange;
        Vector2 rightEdge = AngleToDir(baseAngle + halfAngle) * coneRange;

        Gizmos.color = new Color(1f, 1f, 0f, 0.4f);
        Gizmos.DrawLine(origin, origin + leftEdge);
        Gizmos.DrawLine(origin, origin + rightEdge);
        // Draw arc approximation
        int steps = 20;
        for (int i = 0; i < steps; i++)
        {
            float a1 = baseAngle - halfAngle + (coneAngle / steps) * i;
            float a2 = baseAngle - halfAngle + (coneAngle / steps) * (i + 1);
            Gizmos.DrawLine(origin + AngleToDir(a1) * coneRange,
                            origin + AngleToDir(a2) * coneRange);
        }
    }

    Vector2 AngleToDir(float degrees)
    {
        float rad = degrees * Mathf.Deg2Rad;
        return new Vector2(Mathf.Cos(rad), Mathf.Sin(rad));
    }
}