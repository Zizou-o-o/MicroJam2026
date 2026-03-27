using UnityEngine;
using TMPro;


public class TortureSpawner : MonoBehaviour
{
    public GameObject fallingObjectprefab;
    public TextMeshProUGUI timerText; // we gonna drag the TimerText here in the inspector

    public float spawnRate = 1.5f;
    public float screenWidth = 8f;
    public float spawnHeight = 6f;//just above the camera
    public float stageTime = 60f;//60s
    private bool stageActive = true;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        InvokeRepeating("SpawnHazard",2f,spawnRate); //hadi starts the loop waits 2s and then tdir appell l spawn hazard in each spawnrate

        
    }

    void SpawnHazard (){
        if (!stageActive) return;

        //pick a random position X (left to right)
        float randomX = Random.Range(-screenWidth, screenWidth);

        //create a spawn position vector
        Vector3 spawnPos = new Vector3(randomX, spawnHeight,0);

        //and now create the object in the game fr
        Instantiate(fallingObjectprefab , spawnPos , Quaternion.identity);
        
        
        

    }

    // Update is called once per frame
    void Update()
    {
        if(stageActive) {
            //count down
            stageTime-=Time.deltaTime;
            timerText.text = " SURVIVE " + Mathf.CeilToInt(stageTime).ToString() + " s";
            if (stageTime<=0){
                EndStage();
            }
        }
    }

    void EndStage(){
        stageActive =false;
        CancelInvoke("SpawnHazard"); //hadi thebs spawning
        timerText.text = "MEMORY RECOVERED";
        //tell the player they survive
        Debug.Log("Stage Clear! Transitioning to Memory...");
    }
}
