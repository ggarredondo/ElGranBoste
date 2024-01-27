using UnityEngine;
using UnityEngine.InputSystem;

public class InputController : MonoBehaviour
{
    private Vector2 movementDirection;
    public event System.Action OnPressJoke, OnReleaseJoke;
    public event System.Action<float> OnMouseWheel;

    private void Awake()
    {
        movementDirection = Vector2.zero;
    }

    public void PressJoke(InputAction.CallbackContext context)
    {
        if (context.performed) OnPressJoke?.Invoke();
        else if (context.canceled) OnReleaseJoke?.Invoke();
    }
    public void PressMovement(InputAction.CallbackContext context) => movementDirection = context.ReadValue<Vector2>();

    public void MouseWheel(InputAction.CallbackContext context) 
    {
        if (context.performed)
        {
            OnMouseWheel?.Invoke(context.ReadValue<float>());
        }
    }

    public ref readonly Vector2 MovementDirection => ref movementDirection;
}
