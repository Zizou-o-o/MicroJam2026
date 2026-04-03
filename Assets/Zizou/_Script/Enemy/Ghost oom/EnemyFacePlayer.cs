using UnityEngine;

/// <summary>
/// Attach to any enemy — flips the sprite to always face the player.
/// Works exactly like Boo from Mario (turns to face you).
///
/// SETUP:
///   1. Attach this script to your enemy GameObject
///   2. Drag the Player into the Player field in the Inspector
///   (or leave it empty — it will auto find the Player tag)
/// </summary>
public class EnemyFacePlayer : MonoBehaviour
{
    public Transform player;

    private SpriteRenderer sr;

    void Awake()
    {
        sr = GetComponent<SpriteRenderer>();

        if (player == null)
        {
            GameObject p = GameObject.FindGameObjectWithTag("Player");
            if (p != null) player = p.transform;
        }
    }

    void Update()
    {
        if (player == null || sr == null) return;

        // If player is to the left → flip, if to the right → don't flip
        sr.flipX = player.position.x < transform.position.x;
    }
}