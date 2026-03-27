
using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;
public class PlayerMouvement: MonoBehaviour
{
    [Header("Movement Settings")]
    public float movSpeed = 20f;
    private Rigidbody2D rb;
    [SerializeField]
    public InputActionReference moveAction; // link move here

    // for the jumping mouvement
    public float jumpForce = 14f;
    private bool isGrounded = false;
    private SpriteRenderer sprite;

    //for dashing 
    public float dashPower = 20f;
    public float dashDuration = 0.2f;
    public float dashCooldown = 1f;
    private bool isDashing ;
    private bool canDash = true;
    private float move;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        sprite = GetComponent<SpriteRenderer>();

       
        
    }

   void Update()
    {
         if (isDashing) return; //stop normal mouvement while dashing
         if(Keyboard.current.fKey.wasPressedThisFrame && canDash)
        {
            OnDash();
        }
        //direct reading
        float moveX = moveAction.action.ReadValue<Vector2>().x;
        rb.linearVelocity = new Vector2(moveX * movSpeed , rb.linearVelocity.y);
          //makes the person looks were he walks
       if(moveX > 0) transform.localScale = new Vector3(1,1,1);
       else if (moveX < 0) transform.localScale = new Vector3(-1,1,1);

      

      

       
    }

    public void OnDash()
    {
        if (canDash)
        {
            StartCoroutine(PerformDash());
        }
    }

    private IEnumerator PerformDash()
    {
        canDash = false;
        isDashing = true;
        //apply  spped f direction ta3 sam
        float dashDirection = transform.localScale.x;
        rb.linearVelocity = new Vector2(dashDirection *dashPower , 0f);

        //tell the riidbody to ignore gravity  during the dash time
        float originalGravity = rb.gravityScale;
        rb.gravityScale = 0f;

        //start the host trail
        GhostTrial ghostTrial = GetComponent<GhostTrial>();
        if(ghostTrial != null)
        {
            ghostTrial.StartGhostTrial(dashDuration);

        }

        yield return new WaitForSeconds(dashDuration);

        rb.gravityScale = originalGravity;

        //if he is touching a wall he slips = kill the X velocity so he falls staright down


        rb.linearVelocity = new Vector2(0 , rb.linearVelocity.y);
        isDashing = false;
        //cooldown before next dash

        yield return new WaitForSeconds (dashCooldown);
        canDash = true;

        
      

    }

    public void OnJump(){
        if(isGrounded){
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
            isGrounded = false;
        }
    }

    
    // Update is called once per frame
    private void OnCollisionEnter2D (Collision2D collision)
    {
        if(collision.gameObject.CompareTag("Ground")){
            isGrounded = true;


        }
        
        
    }
}
