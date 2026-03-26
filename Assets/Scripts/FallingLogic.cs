using UnityEngine;

public class FallingLogic : MonoBehaviour
{
    public float fallSpeed = 3f;
    Transform player;
    bool hasHitFloor = false;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        player = GameObject.Find("Player").transform;
    }

    // Update is called once per frame
    void Update()
    {
       
        //check the distance collision
        float distance = Vector2.Distance(transform.position,player.position);

        if (distance < 0.7f && !hasHitFloor){
            hasHitFloor = true;
            Debug.Log("sam was crushed");
            Destroy(gameObject);
        }
        //clean up if it misses sam and falls off screen
        if(transform.position.y < -6f){
            Destroy(gameObject);
        }
        
    }
}
