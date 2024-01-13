using UnityEngine;

public class PlayerLook : MonoBehaviour
{
    private PlayerController _playerController;
    private PlayerAnimator _anim;

    // cam
    private Camera _mainCam;
    private CameraController _camController;

    // rotate
    private Vector2 mousePos;
    private Vector3 rotDir;
    private Quaternion targetDir;

    [Header("RotateValue")]
    [SerializeField] private float rotXSpeed; // x�� ȸ�� �ӵ�
    [SerializeField] private float rotYSpeed; // y�� ȸ�� �ӵ�
    [SerializeField] private float limitMinX = -50; // �Ʒ� 50������
    [SerializeField] private float limitMaxX = 80;  // �� 80������

    [Header("Component")]
    [SerializeField] private Transform camPos; // pelvis -> spine 1, 2, 3 -> neck -> head

    private bool CanZoomIn => 
        (false == _playerController.IsShooting) && 
        (false == _playerController.IsMoving) && 
        (false == _playerController.IsReloading);

    private void Awake()
    {
        _mainCam = Camera.main;
        _camController = _mainCam.gameObject.GetComponent<CameraController>();
        _anim = GetComponent<PlayerAnimator>();
        _playerController = GetComponent<PlayerController>();
    }

    private void Update()
    {
        // rotation���� x���� ����ȸ��, y���� �¿�ȸ����
        // ���� ȸ�� ���� ���� -> �÷��̾� �ٵ� �����¿� ȸ��
        // ī�޶� �ݵ� ����

        rotDir.x = Mathf.Clamp(rotDir.x, limitMinX, limitMaxX);
        targetDir = Quaternion.Euler(rotDir.x, rotDir.y, 0);
        _camController.originalRotation = targetDir; 
    }

    private void LateUpdate()
    {   
        // �÷��̾� Aim
        transform.rotation = targetDir;

        // ī�޶� �����¿� ȸ�� -> ī�޶� ��ġ�� ������Ʈ
        if (_camController.IsShaking)
        {
            _camController.UpdateCameraPositionRotation(camPos.position, null);
        }
        else
        {
            _camController.UpdateCameraPositionRotation(camPos.position, targetDir);
        }
    }

    public void SetMousePos(Vector2 value)
    {
        mousePos = value;

        rotDir.y += mousePos.x * rotXSpeed * Time.deltaTime; // �¿� 
        rotDir.x -= mousePos.y * rotYSpeed * Time.deltaTime; // ����
    }

    public void StartAimMode()
    {
        if (CanZoomIn)
        {
            _camController.ZoomIn();
            _anim.PlayAnimation("aim");
        }
    }

    public void EndAimMode()
    {
        _camController.ZoomOut();
        _anim.StopAnimation("aim");
    }
}
