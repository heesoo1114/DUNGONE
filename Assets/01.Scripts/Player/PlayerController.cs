using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private InputReader _inputReader;

    private PlayerHealth _playerHealth;
    private PlayerMovement _playerMovement;
    private PlayerLook _playerLook;
    private PlayerAttack _playerAttack;

    private void Awake()
    {
        _playerHealth = GetComponent<PlayerHealth>();
        _playerMovement = GetComponent<PlayerMovement>();
        _playerLook = GetComponent<PlayerLook>();
        _playerAttack = GetComponent<PlayerAttack>();
    }

    private void Start()
    {
        _playerHealth.playerDieEvent += BlockInput;

        // Input
        _inputReader.MovementEvent += _playerMovement.SetMovement;
        _inputReader.FastMoveEvent += _playerMovement.SetFastMovement;
        _inputReader.InitMoveEvent += _playerMovement.SetNormalMovement;
        
        _inputReader.ShootEvent += _playerAttack.TryShoot;
        _inputReader.ReloadEvent += _playerAttack.TryReload;

        _inputReader.LookEvent += _playerLook.SetMousePos;
        _inputReader.StartAimEvent += _playerLook.StartAimMode;
        _inputReader.StartAimEvent += _playerMovement.SetSlowMovement;
        _inputReader.EndAimEvent += _playerLook.EndAimMode;
        _inputReader.EndAimEvent += _playerMovement.SetNormalMovement;

        // 입력 활성화
        _inputReader.ActivateInput();
    }

    private void OnDestroy()
    {
        _inputReader.MovementEvent -= _playerMovement.SetMovement;
        _inputReader.FastMoveEvent -= _playerMovement.SetFastMovement;
        _inputReader.InitMoveEvent -= _playerMovement.SetNormalMovement;

        _inputReader.ShootEvent -= _playerAttack.TryShoot;
        _inputReader.ReloadEvent -= _playerAttack.TryReload;

        _inputReader.LookEvent -= _playerLook.SetMousePos;
        _inputReader.StartAimEvent -= _playerLook.StartAimMode;
        _inputReader.StartAimEvent -= _playerMovement.SetSlowMovement;
        _inputReader.EndAimEvent -= _playerLook.EndAimMode;
        _inputReader.EndAimEvent -= _playerMovement.SetNormalMovement;

        _inputReader.DeactivateInput();
    }

    public void BlockInput()
    {
        // 입력 비활성화
        _inputReader.DeactivateInput();
    }
}
