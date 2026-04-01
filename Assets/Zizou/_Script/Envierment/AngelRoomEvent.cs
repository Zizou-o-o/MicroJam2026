using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class AngelRoomEvent : MonoBehaviour
{
    [Header("Timing")]
    public float timeBeforeEvent = 25f;   // seconds before blackout starts

    [Header("Flicker Settings")]
    public int flickerCount = 6;     // how many times it flickers
    public float flickerSpeed = 0.08f; // how fast each flicker is

    [Header("Blackout")]
    public float blackoutDuration = 1.2f; // how long it stays fully black

    [Header("UI — Black Overlay")]
    public Image blackOverlay;            // full screen black UI image

    [Header("Corner Positions (assign 4 empty GameObjects)")]
    public Transform cornerTopLeft;
    public Transform cornerTopRight;
    public Transform cornerBottomLeft;
    public Transform cornerBottomRight;

    [Header("Angels in the scene")]
    public WeepingAngele2D[] angels;        

    private bool eventTriggered = false;

    void Start()
    {
        // Make sure overlay starts invisible
        if (blackOverlay != null)
            SetOverlayAlpha(0f);

        // Auto find angels if not assigned
        if (angels == null || angels.Length == 0)
            angels = FindObjectsByType<WeepingAngele2D>(FindObjectsSortMode.None);

        Invoke(nameof(StartBlackoutEvent), timeBeforeEvent);
    }

    void StartBlackoutEvent()
    {
        if (eventTriggered) return;
        eventTriggered = true;
        StartCoroutine(BlackoutSequence());
    }

    IEnumerator BlackoutSequence()
    {
        // ── Phase 1: Flicker like a failing light ──
        for (int i = 0; i < flickerCount; i++)
        {
            SetOverlayAlpha(1f); // lights off
            yield return new WaitForSeconds(flickerSpeed);
            SetOverlayAlpha(0f); // lights on
            yield return new WaitForSeconds(flickerSpeed);
        }

        // Full blackout ──
        SetOverlayAlpha(1f);
        yield return new WaitForSeconds(blackoutDuration);

        // Teleport angels to corners while screen is black ──
        TeleportAngelsToCorders();

        // Small pause then lights back on ──
        yield return new WaitForSeconds(0.3f);
        SetOverlayAlpha(0f);
    }

    void TeleportAngelsToCorders()
    {
        if (angels == null || angels.Length == 0) return;

        // Build corner list
        List<Transform> corners = new List<Transform>();
        if (cornerTopLeft != null) corners.Add(cornerTopLeft);
        if (cornerTopRight != null) corners.Add(cornerTopRight);
        if (cornerBottomLeft != null) corners.Add(cornerBottomLeft);
        if (cornerBottomRight != null) corners.Add(cornerBottomRight);

        if (corners.Count == 0)
        {
            Debug.LogWarning("[AngelRoomEvent] No corners assigned!");
            return;
        }

        // Shuffle corners so assignment is random each time
        ShuffleList(corners);

        // Assign one angel per corner — max 4 angels
        for (int i = 0; i < angels.Length; i++)
        {
            if (angels[i] == null) continue;
            Transform corner = corners[i % corners.Count];
            angels[i].transform.position = corner.position;
        }

        Debug.Log($"[AngelRoomEvent] Teleported {angels.Length} angel(s) to corners!");
    }

    void SetOverlayAlpha(float alpha)
    {
        if (blackOverlay == null) return;
        Color c = blackOverlay.color;
        c.a = alpha;
        blackOverlay.color = c;
    }

    void ShuffleList(List<Transform> list)
    {
        for (int i = list.Count - 1; i > 0; i--)
        {
            int rand = Random.Range(0, i + 1);
            Transform temp = list[i];
            list[i] = list[rand];
            list[rand] = temp;
        }
    }

    // Draw corners in scene view so you can see them
    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        DrawCornerGizmo(cornerTopLeft, "TL");
        DrawCornerGizmo(cornerTopRight, "TR");
        DrawCornerGizmo(cornerBottomLeft, "BL");
        DrawCornerGizmo(cornerBottomRight, "BR");
    }

    void DrawCornerGizmo(Transform corner, string label)
    {
        if (corner == null) return;
        Gizmos.DrawWireSphere(corner.position, 0.4f);
#if UNITY_EDITOR
        UnityEditor.Handles.Label(corner.position + Vector3.up * 0.5f, label);
#endif
    }
}
