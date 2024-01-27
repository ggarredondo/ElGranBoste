using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[CreateAssetMenu(fileName = "InputReader", menuName = "Scriptable Objects/Input/Input Reader")]
public class InputReader : ScriptableObject
{
    [SerializeField] private List<InputActionReference> holdInputActions;

    public event System.Action MouseClickEvent;
    public event System.Action MenuBackEvent;
    public event System.Action ChangeRightMenuEvent;
    public event System.Action ChangeLeftMenuEvent;
    public event System.Action StartPauseMenuEvent;
    public event System.Action<Vector2> ScrollbarEvent;

    public event System.Action SelectMenuEvent;
    public event System.Action ChangeCamera;
    public event System.Action<Vector2> DPAdEvent;

    public event System.Action<Vector2> Movement;
    public event System.Action<InputAction.CallbackContext> Jump;
    public event System.Action<InputAction.CallbackContext> Boost;
    public event System.Action<InputAction.CallbackContext> RotateRightPress;
    public event System.Action<InputAction.CallbackContext> RotateLeftPress;
    public event System.Action<InputAction.CallbackContext> RotateRightRelease;
    public event System.Action<InputAction.CallbackContext> RotateLeftRelease;

    public Dictionary<InputAction, System.Action<InputAction.CallbackContext>> HoldEvents;

    public void Initialize()
    {
        HoldEvents = new();
        holdInputActions.ForEach(a => HoldEvents.Add(a.action, null));
    }

    public void OnScrollbar(InputAction.CallbackContext context)
    {
        ScrollbarEvent?.Invoke(context.ReadValue<Vector2>());
    }

    public void OnMouseClick(InputAction.CallbackContext context)
    {
        MouseClickEvent?.Invoke();
    }

    public void OnMenuBack(InputAction.CallbackContext context)
    {
        if (context.performed) MenuBackEvent?.Invoke();
    }

    public void OnChangeRightMenu(InputAction.CallbackContext context)
    {
        if (context.performed) ChangeRightMenuEvent?.Invoke();
    }

    public void OnChangeLeftMenu(InputAction.CallbackContext context)
    {
        if (context.performed) ChangeLeftMenuEvent?.Invoke(); 
    }

    public void OnStartPauseMenu(InputAction.CallbackContext context)
    {
        if (context.performed) StartPauseMenuEvent?.Invoke();
    }

    public void OnStartHold(InputAction.CallbackContext context)
    {
        HoldEvents[context.action]?.Invoke(context);
    }

    public void OnSelectMenu(InputAction.CallbackContext context)
    {
        if (context.performed) SelectMenuEvent?.Invoke();
    }

    public void OnDPAd(InputAction.CallbackContext context)
    {
        if (context.performed) DPAdEvent?.Invoke(context.ReadValue<Vector2>());
    }

    public void OnChangeCamera(InputAction.CallbackContext context)
    {
        if (context.performed) ChangeCamera?.Invoke();
    }

    public void OnMovement(InputAction.CallbackContext context)
    {
        if (context.performed) Movement?.Invoke(context.ReadValue<Vector2>());
        if (context.canceled) Movement?.Invoke(new Vector2(0,0));
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        Jump?.Invoke(context);
    }

    public void OnBoost(InputAction.CallbackContext context)
    {
        Boost?.Invoke(context);
    }

    public void OnRotateRightPress(InputAction.CallbackContext context)
    {
        if(context.performed) RotateRightPress?.Invoke(context);
    }

    public void OnRotateLeftPress(InputAction.CallbackContext context)
    {
        if (context.performed) RotateLeftPress?.Invoke(context);
    }

    public void OnRotateRightRelease(InputAction.CallbackContext context)
    {
        if (context.performed) RotateRightRelease?.Invoke(context);
    }

    public void OnRotateLeftRelease(InputAction.CallbackContext context)
    {
        if (context.performed) RotateLeftRelease?.Invoke(context);
    }
}
