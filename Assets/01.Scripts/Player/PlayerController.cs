using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private InputReader _inputReader;

    private PlayerHealth _playerHealth;
    private PlayerMovement _playerMovement;
    private PlayerLook _playerLook;
    private PlayerAttack _playerAttack;
    private CameraController _cameraController;

    // public property
    public bool IsMoving => _playerMovement.IsMoving;
    public bool IsAiming => _cameraController.IsAiming;
    public bool IsShooting => _playerAttack.IsShooting;
    public bool IsReloading => _playerAttack.IsRealoding;

    private void Awake()
    {
        _playerHealth = GetComponent<PlayerHealth>();
        _playerMovement = GetComponent<PlayerMovement>();
        _playerLook = GetComponent<PlayerLook>();
        _playerAttack = GetComponent<PlayerAttack>();
        _cameraController = FindObjectOfType<CameraController>();
    }

    private void Start()
    {
        _playerHealth.playerDieEvent += BlockInput;

        // Movement Input
        _inputReader.MovementEvent += _playerMovement.SetMovement;
        _inputReader.FastMoveEvent += _playerMovement.SetFastMovement;
        _inputReader.InitMoveEvent += _playerMovement.SetNormalMovement;
        
        // Attack and Reload Input
        _inputReader.ShootEvent += _playerAttack.TryShoot;
        _inputReader.ReloadEvent += _playerAttack.TryReload;

        // Aim Input
        _inputReader.LookEvent += _playerLook.SetMousePos;
        _inputReader.StartAimEvent += _playerLook.StartAimMode;
        _inputReader.StartAimEvent += _playerMovement.SetSlowMovement;
        _inputReader.EndAimEvent += _playerLook.EndAimMode;
        _inputReader.EndAimEvent += _playerMovement.SetNormalMovement;

        // �Է� Ȱ��ȭ
        _inputReader.ActivateInput();
    }

    private void OnDestroy()
    {
        _playerHealth.playerDieEvent -= BlockInput;

        // Movement Input
        _inputReader.MovementEvent -= _playerMovement.SetMovement;
        _inputReader.FastMoveEvent -= _playerMovement.SetFastMovement;
        _inputReader.InitMoveEvent -= _playerMovement.SetNormalMovement;

        // Attack and Reload Input
        _inputReader.ShootEvent -= _playerAttack.TryShoot;
        _inputReader.ReloadEvent -= _playerAttack.TryReload;

        // Aim Input
        _inputReader.LookEvent -= _playerLook.SetMousePos;
        _inputReader.StartAimEvent -= _playerLook.StartAimMode;
        _inputReader.StartAimEvent -= _playerMovement.SetSlowMovement;
        _inputReader.EndAimEvent -= _playerLook.EndAimMode;
        _inputReader.EndAimEvent -= _playerMovement.SetNormalMovement;

        _inputReader.DeactivateInput();
    }

    public void BlockInput()
    {
        // �Է� ��Ȱ��ȭ
        _inputReader.DeactivateInput();
    }

}
