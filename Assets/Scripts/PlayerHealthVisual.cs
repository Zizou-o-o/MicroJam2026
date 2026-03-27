using UnityEngine;

public class PlayerHealthVisual : MonoBehaviour
{
    //an array bach yerfed the 5 heart images
    public GameObject[]  hearts;
    private int currentHealth;
    //to prevent getting hit by 5 objects at once 
    private float lastHitTime;
    public float invincibilityDuration = 1f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        currentHealth = hearts.Length; //hadi = 5 
    }

    public void TakeDamage(){
        //check for invincibility timer
        if(Time.time < lastHitTime + invincibilityDuration) return;
        
        if(currentHealth > 0 ){
            currentHealth-- ;

            //hide the heart at the current index  , if  health was 5 than  we hide heart[4] hadak huwa the 5th heart index yebda m 0
            hearts[currentHealth].SetActive(false);


            lastHitTime = Time.time;
            Debug.Log("heart lost , remaining " + currentHealth);

            if(currentHealth <= 0){
                Die();
            }
        }
        StartCoroutine(CameraEffects.instance.Shake(0.2f,0.15f));
    }

    void Die(){
        Debug.Log("sam is lost in the loop bahahah");
        //stop the game 
        Time.timeScale = 0;
    }


    private void OnTriggerEnter2D(Collider2D other){
        if (other.CompareTag("Enemy")){
            TakeDamage();
            Destroy(other.gameObject); // destroy the failing object
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
