using System.Collections;
using TMPro;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private CharacterController _characterController;
    private PlayerAnimator _anim;

    private Transform _rootTrm;

    [Header("MovementValue")]
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float fastMoveSpeed = 8f;
    [SerializeField] private float jumpForce = 5f;
    [SerializeField] private float gravity = -9.8f;
    [SerializeField] private float gravityMultiplier = 1f;
    private float initMoveSpeed;

    [Header("MovementValue")]
    [SerializeField] private float speedLerpChangeSpeed;
    private Coroutine speedChangeCor = null;

    private Vector2 _inputDirection;
    private Vector3 _movementVelocity;
    public Vector3 MovementVelocity => _movementVelocity;
    private float _verticalVelocity;

    public bool IsGround => _characterController.isGrounded;
    public bool IsMoveInputIn => _movementVelocity != Vector3.zero;
    public bool ActiveMove { get; private set; } = true;

    private bool isMoving = false;

    private void Awake()
    {
        _characterController = GetComponent<CharacterController>();
        _anim = GetComponent<PlayerAnimator>();
        _rootTrm = transform;
        initMoveSpeed = moveSpeed;
    }

    public void SetMovement(Vector2 value)
    {
        _inputDirection = value;
    }

    #region FastAndSlow

    public void SetFastMovement()
    {
        if (speedChangeCor != null)
        {
            StopCoroutine(speedChangeCor);
        }
        speedChangeCor = StartCoroutine(LerpSpeedCor(fastMoveSpeed, speedLerpChangeSpeed));
    }

    public void SetNormalMovement()
    {
        if (speedChangeCor != null)
        {
            StopCoroutine(speedChangeCor);
        }
        speedChangeCor = StartCoroutine(LerpSpeedCor(initMoveSpeed, speedLerpChangeSpeed));
    }

    private IEnumerator LerpSpeedCor(float targetSpeed, float changeSpeed)
    {
        float timer = 0f;
        float value = 0f;

        while (true)
        {
            timer += Time.deltaTime;
            value = timer / changeSpeed;
            moveSpeed = Mathf.Lerp(moveSpeed, targetSpeed, value);

            if (value >= 0.9f)
            {
                moveSpeed = targetSpeed;
                break;
            }

            yield return null;
        }

        speedChangeCor = null;
    }

    #endregion

    private void CalculatePlayerMovement()
    {
        _movementVelocity = (_rootTrm.forward * _inputDirection.y + _rootTrm.right * _inputDirection.x) * moveSpeed * Time.fixedDeltaTime;

        if (IsMoveInputIn && !isMoving)
        {
            // _anim.SetBlendValue(moveSpeed);
            _anim.PlayAnimation("move");
            isMoving = true;    
        }
        else if (!IsMoveInputIn && isMoving)
        {
            _anim.StopAnimation("move");
            isMoving = false;
        }
    }

    public void StopImmediately()
    {
        _movementVelocity = Vector3.zero;
    }

    public void SetMovement(Vector3 value)
    {
        _movementVelocity = new Vector3(value.x, 0, value.y);
        _verticalVelocity = value.y;
    }

    private void ApplyGravity()
    {
        if (IsGround && _verticalVelocity < 0)
        {
            _verticalVelocity = -1;
        }
        else
        {
            _verticalVelocity += gravity * gravityMultiplier * Time.fixedDeltaTime;
        }
        _movementVelocity.y = _verticalVelocity;
    }

    private void Move()
    {
        _characterController.Move(_movementVelocity);
    }

    public void Jump()
    {
        if (_characterController.isGrounded)
        {
            _verticalVelocity = jumpForce * gravityMultiplier * Time.fixedDeltaTime;
        }
    }

    private void FixedUpdate()
    {
        if (ActiveMove)
        {
            CalculatePlayerMovement();
        }
        ApplyGravity();
        Move();
    }
}