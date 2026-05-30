using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed;
    public Transform orientation;
    float horizontalInput;
    float verticalInput; 
    Vector3 movementDir;
    Rigidbody rb;

    public float playerHeight;
    public LayerMask groundMask;
    bool grounded;
    [SerializeField] private float groundDrag;

    void Start()
    {
        rb = transform.GetComponent<Rigidbody>();
        rb.freezeRotation = true;
    }

    private void InputHandle()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");
    }

    private void MovePlayer()
    {
        movementDir = orientation.forward * verticalInput + orientation.right * horizontalInput;
        rb.AddForce(movementDir * moveSpeed , ForceMode.Force);
    }

    void FixedUpdate()
    {
        MovePlayer();
    }

    void Update()
    {
        //verifie si le joueur est au sol
        grounded = Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.5f + 0.2f, groundMask);

        InputHandle();


        //applique le drag si le joueur est au sol
        if(grounded)
            rb.linearDamping = groundDrag;
        else
            rb.linearDamping = 0;
    }
}
