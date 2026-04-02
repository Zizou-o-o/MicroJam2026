using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class FlashlightVisibility : MonoBehaviour
{
    [Header("References")]
    public Flashlight flashlight;
    public SpriteMask coneMask;      
    public SpriteRenderer darkOverlay;    
    public AngelRoomEvent angelRoomEvent; 

    [Header("Flicker Settings")]
    public float flickerInterval = 25f;  // how often the flicker happens
    public int flickerCount = 5;    // how many times it flickers
    public float flickerSpeed = 0.07f;// how fast each flicker is
    public float blackoutDuration = 1f;  // how long full dark lasts

    [Header("Angels (to teleport to corners)")]
    public WeepingAngele2D[] angels;

    [Header("Corners")]
    public Transform cornerTopLeft;
    public Transform cornerTopRight;
    public Transform cornerBottomLeft;
    public Transform cornerBottomRight;

    void Start()
    {
        // Auto find angels if not assigned
        if (angels == null || angels.Length == 0)
            angels = FindObjectsByType<WeepingAngele2D>(FindObjectsSortMode.None);

        // Start the flicker loop
        StartCoroutine(FlickerLoop());
    }

    //fires every 25 seconds 
    IEnumerator FlickerLoop()
    {
        while (true)
        {
            yield return new WaitForSeconds(flickerInterval);
            yield return StartCoroutine(DoFlickerSequence());
        }
    }

    IEnumerator DoFlickerSequence()
    {
        // Phase 1: Flicker on and off
        for (int i = 0; i < flickerCount; i++)
        {
            SetFlashlight(false); // off
            yield return new WaitForSeconds(flickerSpeed);
            SetFlashlight(true);  // on
            yield return new WaitForSeconds(flickerSpeed);
        }

        // Phase 2: Full blackout — turn off flashlight completely
        SetFlashlight(false);
        yield return new WaitForSeconds(blackoutDuration);

        // Phase 3: Teleport angels to corners while dark
        TeleportAngelToCorners();

        // Phase 4: Small pause then flashlight back on
        yield return new WaitForSeconds(0.2f);
        SetFlashlight(true);
    }

    // ── Toggle flashlight cone mask on/off ───────────────────────
    void SetFlashlight(bool on)
    {
        if (coneMask != null)
            coneMask.enabled = on;
    }

    // Teleport angels to corners 
    void TeleportAngelToCorners()
    {
        if (angels == null || angels.Length == 0) return;

        List<Transform> corners = new List<Transform>();
        if (cornerTopLeft != null) corners.Add(cornerTopLeft);
        if (cornerTopRight != null) corners.Add(cornerTopRight);
        if (cornerBottomLeft != null) corners.Add(cornerBottomLeft);
        if (cornerBottomRight != null) corners.Add(cornerBottomRight);

        if (corners.Count == 0) return;

        // Shuffle corners
        for (int i = corners.Count - 1; i > 0; i--)
        {
            int rand = Random.Range(0, i + 1);
            Transform temp = corners[i];
            corners[i] = corners[rand];
            corners[rand] = temp;
        }

        // One angel per corner
        for (int i = 0; i < angels.Length; i++)
        {
            if (angels[i] == null) continue;
            angels[i].transform.position = corners[i % corners.Count].position;
        }

        Debug.Log($"[FlashlightVisibility] Teleported {angels.Length} angel(s) to corners!");
    }

    // Draw corners in scene view
    void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        DrawGizmo(cornerTopLeft, "TL");
        DrawGizmo(cornerTopRight, "TR");
        DrawGizmo(cornerBottomLeft, "BL");
        DrawGizmo(cornerBottomRight, "BR");
    }

    void DrawGizmo(Transform t, string label)
    {
        if (t == null) return;
        Gizmos.DrawWireSphere(t.position, 0.4f);
#if UNITY_EDITOR
        UnityEditor.Handles.Label(t.position + Vector3.up * 0.5f, label);
#endif
    }
}