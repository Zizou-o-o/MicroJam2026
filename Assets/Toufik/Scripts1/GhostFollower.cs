
using UnityEngine;

public class GhostFollower : MonoBehaviour
{
    public Transform Player;//drag sam here
    public float speed = 3f;

    private Rigidbody2D rb;

    private GeneratorCore gn;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        gn = GameObject.FindFirstObjectByType<GeneratorCore>();
    }
    // Update is called once per frame
    void FixedUpdate()
    {
        if(gn != null && gn.RepairsDone >= gn.totalRepairsNeeded)
        {
            rb.linearVelocity = Vector2.zero; //stop physics mouvement
            return; // skip the rest of the mouvemenet code 
            
        }



        if (Player != null && rb != null){
            Vector2 direction = (Player.position - transform.position).normalized;

            rb.linearVelocity = direction *speed;
            // rotate face the player
            float angle = Mathf.Atan2(direction.y , direction.x) * Mathf.Rad2Deg;
            rb.SetRotation(angle);

        }
        
    }

    private void OnCollisionEnter2D(Collision2D collision){
        if(collision.gameObject.CompareTag("Player")){
            GameManager gm = FindFirstObjectByType<GameManager>();
            if(gm != null) {
                gm.GameOver(); 
            }
        }

    }
}
