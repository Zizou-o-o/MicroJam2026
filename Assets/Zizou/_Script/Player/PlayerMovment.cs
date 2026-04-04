using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed = 5f;

    // Tracks last movement direction for flashlight & sprite facing
    [HideInInspector] public Vector2 facingDirection = Vector2.down;

    private Rigidbody2D rb;
    private Animator animator; //optional if we needed a animation

    // Direction enum for clean sprite swapping if needed
    public enum FaceDir { Up, Down, Left, Right }
    [HideInInspector] public FaceDir currentFaceDir = FaceDir.Down;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>(); 
    }

    void Update()
    {
        HandleInput();
    }

    void HandleInput()
    {
        float h = Input.GetAxisRaw("Horizontal"); 
        float v = Input.GetAxisRaw("Vertical");  

        Vector2 moveDir = new Vector2(h, v).normalized;
        if (moveDir.magnitude > 0) {
            animator.SetBool("walk", true);
        }
        else
        {
            animator.SetBool("walk", false);
        }


        rb.linearVelocity = moveDir * moveSpeed;

        // Update facing only when actually moving
        if (moveDir != Vector2.zero)
        {
            facingDirection = moveDir;
            UpdateFacing(moveDir);
        }
    }

    void UpdateFacing(Vector2 dir)
    {
        // Determine dominant axis
        if (Mathf.Abs(dir.x) >= Mathf.Abs(dir.y))
        {
            currentFaceDir = dir.x > 0 ? FaceDir.Right : FaceDir.Left;
        }
        else
        {
            currentFaceDir = dir.y > 0 ? FaceDir.Up : FaceDir.Down;
        }

        //Animator approach
        if (animator != null)
        {
            animator.SetFloat("MoveX", dir.x);
            animator.SetFloat("MoveY", dir.y);
            animator.SetBool("IsMoving", true);
        }

        // Sprite flip for left/right 
        // If your sprite faces RIGHT by default:
        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        if (sr != null && Mathf.Abs(dir.x) >= Mathf.Abs(dir.y))
        {
            sr.flipX = dir.x < 0;
        }
    }

    // Call this from Animator or wherever movement stops
    void OnMovementStop()
    {
        if (animator != null)
            animator.SetBool("IsMoving", false);
    }
}