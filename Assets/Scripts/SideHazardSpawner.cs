
using UnityEngine;
using System.Collections;

public class SideHazardSpawner : MonoBehaviour
{

    [Header("Sync with Main Timer")]
    public float timer = 60f;
    

   

    [Header("Projectile Settings")]
    public GameObject sideProjectilePrefab;
    public float spawnInterval = 4f; // Time between attacks
    public float spawnHeight = -10.5f;
    public float projectilespeed = 18f;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        StartCoroutine(SideAttackLoop());

        
    }

    void Update()
    {
        //countdown
        if(timer > 0)
        {
            timer -= Time.deltaTime;
        }
        
    }

    IEnumerator SideAttackLoop()
    {
        while (timer> 0)
        {
            yield return new WaitForSeconds(spawnInterval);

            //Randomise left or right
           float spawnX = (Random.value>0.5f) ? -20f : 20f;
           bool fromLeft =(spawnX < 0);

           //luanch 
           if (timer > 0)
            {
                LaunchProjectile(spawnX , spawnHeight , fromLeft);
            }
        }
        Debug.Log("side spawner 60 s ended");
    }

    void LaunchProjectile(float x , float y , bool left)
    {
        GameObject projectile = Instantiate(sideProjectilePrefab , new Vector3(x,y,0) , Quaternion.identity);
        Rigidbody2D rb = projectile.GetComponent<Rigidbody2D>();

        //side projectils shouldnt fall down!
        if (rb != null)
        {
            rb.gravityScale = 0;

        float direction = left ? 1: -1;
        rb.linearVelocity = new Vector2(direction * projectilespeed , 0);
        }

        //clean up memory after 4 seconds
        Destroy(projectile , 4f);

        
    }

    
}
