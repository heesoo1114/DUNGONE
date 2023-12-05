using UnityEngine.InputSystem;
using UnityEngine;
using System;
using static Controls;

[CreateAssetMenu(menuName = "SO/Input")]
public class InputReader : ScriptableObject, IPlayerInputActions
{
    private Controls _keyAction;

    public event Action<Vector2> MovementEvent;
    public event Action<Vector2> LookEvent;

    public Action ShootEvent;
    public Action ReloadEvent;

    public Action StartAimEvent;
    public Action EndAimEvent;

    public Action FastMoveEvent;
    public Action SlowMoveEvent;

    #region Setting

    private void OnEnable()
    {
        if (_keyAction == null)
        {
            _keyAction = new Controls();
            _keyAction.PlayerInput.SetCallbacks(this); // �� SOŬ������ ��ǲ�� �� ����
        }
    }

    public void ActivateInput()
    {
        _keyAction.PlayerInput.Enable(); // �Է� Ȱ��ȭ
    }

    public void DeactivateInput()
    {
        _keyAction.PlayerInput.Disable(); // �Է� ��Ȱ��ȭ
    }

    #endregion

    #region Event

    public void OnMovement(InputAction.CallbackContext context)
    {
        Vector2 input = _keyAction.PlayerInput.Movement.ReadValue<Vector2>();
        MovementEvent?.Invoke(input);
    }

    public void OnLook(InputAction.CallbackContext context)
    {
        Vector2 input = _keyAction.PlayerInput.Look.ReadValue<Vector2>();
        LookEvent?.Invoke(input);
    }

    public void OnShoot(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            ShootEvent?.Invoke();
        }
    }

    public void OnAim(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            StartAimEvent?.Invoke();
        }
        else if (context.canceled)
        {
            EndAimEvent?.Invoke();
        }
    }

    public void OnReload(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            ReloadEvent?.Invoke();
        }
    }

    public void OnRun(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            FastMoveEvent?.Invoke();
        }
        else if (context.canceled)
        {
            SlowMoveEvent?.Invoke();
        }
    }

    #endregion
}
