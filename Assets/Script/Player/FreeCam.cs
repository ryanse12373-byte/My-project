using UnityEngine;

public class FreeCam : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed = 10f;
    public float sprintMultiplier = 3f;
    public float verticalSpeed = 10f;

    [Header("Look")]
    public float mouseSensitivity = 2f;

    private float rotationX;
    private float rotationY;

    void Start()
    {
        Vector3 rot = transform.eulerAngles;
        rotationX = rot.y;
        rotationY = rot.x;
    }

    void Update()
    {
        HandleLook();
        HandleMovement();
    }

    void HandleLook()
    {
        if (Input.GetMouseButton(1))
        {
            Cursor.lockState = CursorLockMode.Locked;

            rotationX += Input.GetAxis("Mouse X") * mouseSensitivity;
            rotationY -= Input.GetAxis("Mouse Y") * mouseSensitivity;
            rotationY = Mathf.Clamp(rotationY, -90f, 90f);

            transform.rotation = Quaternion.Euler(rotationY, rotationX, 0);
        }

        if (Input.GetMouseButtonUp(1))
        {
            Cursor.lockState = CursorLockMode.None;
        }
    }

    void HandleMovement()
    {
        float speed = moveSpeed;

        if (Input.GetKey(KeyCode.LeftShift))
            speed *= sprintMultiplier;

        Vector3 moveDirection = Vector3.zero;

        moveDirection += transform.forward * Input.GetAxisRaw("Vertical");
        moveDirection += transform.right * Input.GetAxisRaw("Horizontal");

        if (Input.GetKey(KeyCode.Space))
            moveDirection += Vector3.up;

        if (Input.GetKey(KeyCode.LeftControl))
            moveDirection += Vector3.down;

        transform.position += moveDirection.normalized * speed * Time.deltaTime;
    }
}