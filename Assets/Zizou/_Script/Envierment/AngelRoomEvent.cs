using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class AngelRoomEvent : MonoBehaviour
{
    [Header("Timing")]
    public float timeBeforeEvent = 25f;

    [Header("Flicker Settings")]
    public int flickerCount = 6;
    public float flickerSpeed = 0.08f;

    [Header("Blackout")]
    public float blackoutDuration = 1.2f;

    [Header("UI — Black Overlay")]
    public Image blackOverlay;

    [Header("Corner Positions")]
    public Transform cornerTopLeft;
    public Transform cornerTopRight;
    public Transform cornerBottomLeft;
    public Transform cornerBottomRight;

    [Header("Angels in the scene")]
    public WeepingAngele2D[] angels;

    // FlashlightDarkness listens to this to enable darkness after blackout
    public event System.Action OnBlackoutEnd;

    private bool eventTriggered = false;

    void Start()
    {
        if (blackOverlay != null)
            SetOverlayAlpha(0f);

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
        // Phase 1: Flicker
        for (int i = 0; i < flickerCount; i++)
        {
            SetOverlayAlpha(1f);
            yield return new WaitForSeconds(flickerSpeed);
            SetOverlayAlpha(0f);
            yield return new WaitForSeconds(flickerSpeed);
        }

        // Phase 2: Full blackout
        SetOverlayAlpha(1f);
        yield return new WaitForSeconds(blackoutDuration);

        // Phase 3: Teleport angels to corners while black
        TeleportAngelsToCorders();

        // Phase 4: Lights back on
        yield return new WaitForSeconds(0.3f);
        SetOverlayAlpha(0f);

        // Phase 5: Fire event — darkness mode starts now
        OnBlackoutEnd?.Invoke();
    }

    void TeleportAngelsToCorders()
    {
        if (angels == null || angels.Length == 0) return;

        List<Transform> corners = new List<Transform>();
        if (cornerTopLeft != null) corners.Add(cornerTopLeft);
        if (cornerTopRight != null) corners.Add(cornerTopRight);
        if (cornerBottomLeft != null) corners.Add(cornerBottomLeft);
        if (cornerBottomRight != null) corners.Add(cornerBottomRight);

        if (corners.Count == 0) { Debug.LogWarning("[AngelRoomEvent] No corners assigned!"); return; }

        ShuffleList(corners);

        for (int i = 0; i < angels.Length; i++)
        {
            if (angels[i] == null) continue;
            angels[i].transform.position = corners[i % corners.Count].position;
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
