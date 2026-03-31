using UnityEngine;

public class PlayerHallwaySpawner : MonoBehaviour
{
    void Start()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
            player.transform.position = transform.position;
        else
            Debug.LogWarning("[PlayerHallwaySpawner] No GameObject tagged 'Player' found!");
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawSphere(transform.position, 0.3f);
#if UNITY_EDITOR
        UnityEditor.Handles.Label(transform.position + Vector3.up * 0.5f, "PLAYER SPAWN");
#endif
    }
}
