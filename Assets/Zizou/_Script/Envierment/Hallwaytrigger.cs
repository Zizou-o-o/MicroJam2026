using UnityEngine;

public class HallwayTrigger : MonoBehaviour
{
    [Header("Which minigame does this drawing lead to?")]
    public int minigameNumber = 1; // 1, 2 or 3

    private bool hasTriggered = false;

    void OnEnable()
    {
        hasTriggered = false;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (hasTriggered) return;
        if (!other.CompareTag("Player")) return;
        if (GameManager1.Instance == null) return;

        // Only fire if this is the next minigame the player needs
        // Drawing 1 → needs 0 wins so far
        // Drawing 2 → needs 1 win so far
        // Drawing 3 → needs 2 wins so far
        if (GameManager1.Instance.MinigamesWonThisHallway != minigameNumber - 1) return;

        hasTriggered = true;
        GameManager1.Instance.EnterMinigame(minigameNumber);
    }

    void OnDrawGizmos()
    {
        BoxCollider2D box = GetComponent<BoxCollider2D>();
        if (box == null) return;

        Gizmos.color = new Color(0f, 1f, 0.4f, 0.35f);
        Gizmos.DrawCube(transform.position + (Vector3)box.offset,
                        new Vector3(box.size.x, box.size.y, 0.1f));
        Gizmos.color = new Color(0f, 1f, 0.4f, 1f);
        Gizmos.DrawWireCube(transform.position + (Vector3)box.offset,
                            new Vector3(box.size.x, box.size.y, 0.1f));
#if UNITY_EDITOR
        UnityEditor.Handles.Label(
            transform.position + Vector3.up,
            $"Drawing → Minigame {minigameNumber}"
        );
#endif
    }
}