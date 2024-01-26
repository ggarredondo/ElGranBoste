using UnityEngine;
using UnityEngine.InputSystem;

public class FirstPersonMovement : MonoBehaviour
{
    private CharacterController controller;
    private Transform camTransform;
    private Vector2 movementVector;
    [SerializeField] private float movementSpeed = 1f;

    private void Awake()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        controller = GetComponent<CharacterController>();
        camTransform = Camera.main.transform;
    }

    private void Update()
    {
        Vector3 forward = Vector3.Cross(camTransform.right, Vector3.up).normalized;
        Vector3 direction = camTransform.right * movementVector.x + forward * movementVector.y;
        transform.LookAt(transform.position + forward);
        controller.Move((direction * movementSpeed + Physics.gravity) * Time.deltaTime);
    }

    public void SetMovementVector(InputAction.CallbackContext context) => movementVector = context.ReadValue<Vector2>();
}
