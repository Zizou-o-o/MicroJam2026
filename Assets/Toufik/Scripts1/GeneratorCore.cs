using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;


public class GeneratorCore : MonoBehaviour
{
    
    [Header("Timers")]
    public float timeToFix = 30f;
    public float timeBetweenBreaks = 20f;
    private float currentTimer;

    [Header("Progression")]
    public int totalRepairsNeeded = 3;
    public int RepairsDone = 0;

    private bool isBroken = true; // starts broken
    private bool hasWon = false;
  
    public TextMeshProUGUI machineStatusText;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        currentTimer = timeToFix;
       
       
    }


    // Update is called once per frame
    void Update()
    {
        if(hasWon) return;
        currentTimer-=Time.deltaTime;

        //generator is broken
        if(isBroken){
            if(machineStatusText)
             machineStatusText.text = "CRITICAL FAILURE!\nTIME LEFT: " + Mathf.CeilToInt(currentTimer) + "s\nGOAL: "+ RepairsDone+"/"+totalRepairsNeeded;
            if(currentTimer <= 0){
                Debug.Log("Boom! GameOver");
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);

        
            } 
        }
        //phase two generator is working
        else{
            if(machineStatusText)
             machineStatusText.text = "SYSTEM STABLE..\nBREAKING IN: "+Mathf.CeilToInt(currentTimer) + "s";
            if(currentTimer <= 0){
                BreakAgain();
            } 
        }
      
        
    }

    public void Repair(SamController sam){

        if(isBroken && sam.hasVicePart){
            sam.hasVicePart= false;
            RepairsDone ++ ;

            if(RepairsDone >= totalRepairsNeeded){
                WinGame();
            } else{
                //fix successful start the 20s timer
                isBroken =false;
                currentTimer = timeBetweenBreaks;
                if(machineStatusText) machineStatusText.text = "REPAIRED! STAY ALERT..";
            }
        }
    }
    void BreakAgain(){
        isBroken = true;
        currentTimer = timeToFix;
        Debug.Log("generator has failed again");
    }
    

    void WinGame(){
        hasWon = true;
        if(machineStatusText) machineStatusText.text = "CORE STABILIZED YOU WON!";

        // stop the monster 
        GameObject monster = GameObject.FindWithTag("Enemy");
        if(monster != null ) monster.SetActive(false);
    }

    
}
