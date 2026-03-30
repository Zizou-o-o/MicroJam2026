using UnityEngine;
using TMPro;
using UnityEngine.UI;
public class ViceGenerator : MonoBehaviour
{
   

   public float timeToCraft = 15f;
   private float timer = 0f;
  
  private bool isCrafting = false;
  private bool isPartReady = false;
  private bool isPlayerInside = false;

  public TextMeshProUGUI uiText;

   void OnTriggerEnter2D(Collider2D other)
   {
    if(other.CompareTag("Player")) isPlayerInside = true;
   }

   void OnTriggerExit2D(Collider2D other){
    isPlayerInside = false;
   }


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    
    // Update is called once per frame
    void Update()
    {
        //start the machine
        if(isPlayerInside && Input.GetKeyDown(KeyCode.E) && !isCrafting && !isPartReady){
            isCrafting = true;
            timer = 0;
            Debug.Log("machine started");

        }

        //automatic crafting (sam can be anywhere)
        if(isCrafting){
            timer += Time.deltaTime;
            if(uiText) uiText.text = "MANUFACTURING " + Mathf.FloorToInt((timer / timeToCraft) * 100) + "%";

            if (timer >= timeToCraft){
                isCrafting = false;
                isPartReady  = true;
                if(uiText) uiText.text = "PART READY - PRESS E TO COLLECT";
            }
        }

        //collection
        if(isPlayerInside && Input.GetKeyDown(KeyCode.E) && isPartReady){
            var sam = GameObject.FindWithTag("Player").GetComponent<SamController>();
            sam.hasVicePart = true;
            isPartReady = false;//reset the machine for next time
            if(uiText)uiText.text = "PART COLLECTED";
        }

        //clear text if sam walks away
        if(!isPlayerInside && !isCrafting && !isPartReady && uiText){
            uiText.text = "" ;
        }
        
    }
}
