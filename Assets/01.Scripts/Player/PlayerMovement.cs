using System.Collections;
using UnityEditor.Build.Reporting;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private PlayerController _playerController;
    private CharacterController _characterController;
    private PlayerAnimator _anim;
    private PlayerWalkSound _walkSoundPlayer;

    private Transform _rootTrm;

    [Header("MovementValue")]
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float fastMoveSpeed = 8f;
    [SerializeField] private float slowMoveSpeed = 3f;
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

    public bool IsMoving { get; private set; } = false;

    private bool CanFastMove => (false == _playerController.IsAiming) && (false == _playerController.IsShooting);

    private void Awake()
    {
        _characterController = GetComponent<CharacterController>();
        _anim = GetComponent<PlayerAnimator>();
        _playerController = GetComponent<PlayerController>();
        _walkSoundPlayer = GetComponentInChildren<PlayerWalkSound>();

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
        if (false == CanFastMove) return;

        if (speedChangeCor != null)
        {
            StopCoroutine(speedChangeCor);
        }
        speedChangeCor = StartCoroutine(LerpSpeedCor(fastMoveSpeed, speedLerpChangeSpeed));
    }

    public void SetSlowMovement()
    {
        if (speedChangeCor != null)
        {
            StopCoroutine(speedChangeCor);
        }
        speedChangeCor = StartCoroutine(LerpSpeedCor(slowMoveSpeed, speedLerpChangeSpeed));
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

        if (IsMoveInputIn && !IsMoving)
        {
            _anim.PlayAnimation("move");
            IsMoving = true;

            // 사운드 재생
            _walkSoundPlayer.PlayWalkSound();
        }
        else if (!IsMoveInputIn && IsMoving)
        {
            _anim.StopAnimation("move");
            IsMoving = false;

            // 사운드 정지
            _walkSoundPlayer.StopWalkSound();
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