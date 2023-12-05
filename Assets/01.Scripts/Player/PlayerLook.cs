using UnityEngine;

public class PlayerLook : MonoBehaviour
{
    private PlayerAnimator _anim;

    // cam
    private Camera _mainCam;
    private CameraController _camController;

    // rotate
    private Vector2 mousePos;
    private Vector3 rotDir;
    private Quaternion targetDir;

    [Header("RotateValue")]
    [SerializeField] private float rotXSpeed; // x축 회전 속도
    [SerializeField] private float rotYSpeed; // y축 회전 속도
    [SerializeField] private float limitMinX = -50; // 아래 50도까지
    [SerializeField] private float limitMaxX = 80;  // 위 80도까지

    [Header("Component")]
    [SerializeField] private Transform camPos; // pelvis -> spine 1, 2, 3 -> neck -> head

    private void Awake()
    {
        _mainCam = Camera.main;
        _camController = _mainCam.gameObject.GetComponent<CameraController>();
        _anim = GetComponent<PlayerAnimator>();
    }

    private void LateUpdate()
    {
        // rotation에서 x축이 상하회전, y축이 좌우회전임
        // 상하 회전 각도 제한 -> 플레이어 바디 상하좌우 회전
        // 카메라 상하좌우 회전 -> 카메라 위치를 업데이트

        rotDir.x = Mathf.Clamp(rotDir.x, limitMinX, limitMaxX);
        targetDir = Quaternion.Euler(rotDir.x, rotDir.y, 0);

        transform.rotation = targetDir;

        _mainCam.transform.rotation = targetDir;
        _mainCam.transform.position = camPos.position;
    }

    public void SetMousePos(Vector2 value)
    {
        mousePos = value;

        rotDir.y += mousePos.x * rotXSpeed * Time.deltaTime; // 좌우 
        rotDir.x -= mousePos.y * rotYSpeed * Time.deltaTime; // 상하
    }

    public void StartAimMode()
    {
        _camController.ZoomIn();
        _anim.PlayAnimation("aim");
    }

    public void EndAimMode()
    {
        _camController.ZoomOut();
        _anim.StopAnimation("aim");
    }
}
