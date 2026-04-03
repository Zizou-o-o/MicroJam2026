using UnityEngine;

public class HallwayEndTrigger : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;
        if (GameManager1.Instance == null) return;

        GameManager1.Instance.OnHallwayEnd();
    }

    void OnDrawGizmos()
    {
        BoxCollider2D box = GetComponent<BoxCollider2D>();
        if (box == null) return;

        Gizmos.color = new Color(1f, 0.3f, 0f, 0.35f);
        Gizmos.DrawCube(transform.position + (Vector3)box.offset,
                        new Vector3(box.size.x, box.size.y, 0.1f));
        Gizmos.color = new Color(1f, 0.3f, 0f, 1f);
        Gizmos.DrawWireCube(transform.position + (Vector3)box.offset,
                            new Vector3(box.size.x, box.size.y, 0.1f));
#if UNITY_EDITOR
        UnityEditor.Handles.Label(
            transform.position + Vector3.up,
            "Hallway End →"
        );
#endif
    }
}
