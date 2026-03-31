using UnityEngine;

public class Flashlight : MonoBehaviour
{
    [Header("Cone Settings")]
    [Range(10f, 120f)] public float coneAngle = 60f;
    public float coneRange = 8f;

    private Vector2 facingDirection = Vector2.right;

    void Update()
    {
        RotateToMouse();
    }

    void RotateToMouse()
    {
        // Get mouse position in world space
        Vector3 mouseWorld = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mouseWorld.z = 0f;

        // Direction from flashlight to mouse
        Vector2 dir = (mouseWorld - transform.position).normalized;
        facingDirection = dir;

        // Rotate flashlight to face mouse
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0f, 0f, angle);
    }
    public bool IsInCone(Vector2 targetPosition)
    {
        Vector2 toTarget = targetPosition - (Vector2)transform.position;
        if (toTarget.magnitude > coneRange) return false;
        return Vector2.Angle(facingDirection, toTarget) <= coneAngle / 2f;
    }

    void OnDrawGizmos()
    {
        Vector2 origin = transform.position;
        Vector2 dir = facingDirection == Vector2.zero ? Vector2.right : facingDirection;
        float baseAngle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        float half = coneAngle / 2f;

        Gizmos.color = new Color(1f, 1f, 0f, 0.4f);
        Gizmos.DrawLine(origin, origin + DirFromAngle(baseAngle - half) * coneRange);
        Gizmos.DrawLine(origin, origin + DirFromAngle(baseAngle + half) * coneRange);

        int steps = 20;
        for (int i = 0; i < steps; i++)
        {
            float a1 = baseAngle - half + (coneAngle / steps) * i;
            float a2 = baseAngle - half + (coneAngle / steps) * (i + 1);
            Gizmos.DrawLine(origin + DirFromAngle(a1) * coneRange,
                            origin + DirFromAngle(a2) * coneRange);
        }
    }

    Vector2 DirFromAngle(float deg)
    {
        float rad = deg * Mathf.Deg2Rad;
        return new Vector2(Mathf.Cos(rad), Mathf.Sin(rad));
    }
}