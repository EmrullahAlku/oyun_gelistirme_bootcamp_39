using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float speed = 5f;

    [Header("Mouse Look")]
    [SerializeField] private float mouseSensitivity = 100f;
    [SerializeField] private Transform cameraTransform;
    private float xRotation = 0f;

    private Rigidbody rb;
    private Vector2 inputMovement;
    private Vector2 inputLook;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        // Lock cursor
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Update()
    {
        // Mouse look
        float mouseX = inputLook.x * mouseSensitivity * Time.deltaTime;
        float mouseY = inputLook.y * mouseSensitivity * Time.deltaTime;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f); // Prevent full head tilt

        cameraTransform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        transform.Rotate(Vector3.up * mouseX);
    }

    private void FixedUpdate()
    {
        Vector3 move = transform.right * inputMovement.x + transform.forward * inputMovement.y;
        rb.MovePosition(rb.position + move * speed * Time.fixedDeltaTime);
    }

    // Input System methods (call these from your InputAction asset)
public void OnLook(InputValue value)
{
    inputLook = value.Get<Vector2>();
}

public void OnMove(InputValue value)
{
    inputMovement = value.Get<Vector2>();
}
}