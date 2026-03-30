using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.InputSystem;

public class SamController : MonoBehaviour
{
    public float speed = 12f;
    private Rigidbody2D rb;
    private Vector2 moveInput;

    
    public bool hasVicePart = false;
    public TextMeshProUGUI StatusText; // UI text object here



    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();        
    }

    public void OnMove(InputValue value){
        moveInput = value.Get<Vector2>();



    }

    public void Update(){
        if(StatusText != null){
            if(hasVicePart){
                StatusText.text = "INVENTORY : [VICE PART]";
            }else{
                StatusText.text = "INVENTORY : [EMPTY]";
            }

        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            CheckInteractions();
        }
    }

    

    // Update is called once per frame
    void FixedUpdate()
    {
        rb.linearVelocity = moveInput *speed;
        
    }

    void CheckInteractions()
    {
        //creates an invisible circle around sam 
        Collider2D [] objectsNearSam = Physics2D.OverlapCircleAll(transform.position, 2.0f);

        foreach (var obj in objectsNearSam)
        {
            if (obj.CompareTag("Generator"))
            {
               GeneratorCore gen = obj.GetComponent<GeneratorCore>();
               if(gen != null)
                {
                    gen.Repair(this);//this= sam
                }
            }
            //get the script from the generator function and call the repair fct
           
        
        }
    }
}
