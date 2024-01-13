using System.Collections;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    private Camera _mainCam;

    [Header("AimMode")]
    [SerializeField] private float aimChangeAnimSpeed = 1;
    private Coroutine aimChangeCor;
    private float initAimValue = 60f;
    private float targetAimvalue = 50f;
    public bool IsAiming { get; private set; }

    // recoil
    // public Vector3 recoilOffset { get; private set; }
    private Quaternion originalCameraRotation;
    private bool isShaking;
    public bool IsShaking 
    { 
        get { return isShaking; } 
        private set { } 
    }

    private void Awake()
    {
        _mainCam = Camera.main; 
    }

    public void UpdateCameraPositionRotation(Vector3? position = null, Quaternion? rotation = null)
    {
        if (position.HasValue)
        {
            _mainCam.transform.position = position.Value;
        }
        if (rotation.HasValue)
        {
            _mainCam.transform.rotation = rotation.Value;
        }
    }

    #region Shake

    public void ApplyRecoil(float magnitude, float duration)
    {
        StartCoroutine(RecoilCor(magnitude, duration));
    }

    public Quaternion originalRotation;
    private IEnumerator RecoilCor(float angle, float duration)
    {
        isShaking = true;

        // originalRotation = transform.localRotation;
        Quaternion lerpedRotation = Quaternion.identity;

        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            float pingPongValue = Mathf.PingPong(elapsedTime * 2f, 1f);
            float lerpedAngle = Mathf.Lerp(0f, angle, pingPongValue);

            lerpedRotation = originalRotation * Quaternion.AngleAxis(lerpedAngle, Vector3.left);

            UpdateCameraPositionRotation(null, lerpedRotation);
            // transform.localRotation = lerpedRotation;

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        transform.localRotation = originalRotation;
        isShaking = false;
    }


    #endregion

    #region AimMode

    // ���ذ�
    public void ZoomIn()
    {
        IsAiming = true;

        if (aimChangeCor != null)
        {
            StopCoroutine(aimChangeCor);
        }
        aimChangeCor = StartCoroutine(AimChangeAnim(targetAimvalue, aimChangeAnimSpeed));
    }

    // ����
    public void ZoomOut()
    {
        IsAiming = false;
        
        if (aimChangeCor != null)
        {
            StopCoroutine(aimChangeCor);
        }
        aimChangeCor = StartCoroutine(AimChangeAnim(initAimValue, aimChangeAnimSpeed));
    }

    private IEnumerator AimChangeAnim(float targetValue, float speed)
    {
        float moveTime = 0;
        float value = 0;

        while (true)
        {
            moveTime += Time.deltaTime;
            value = moveTime / speed;

            _mainCam.fieldOfView = Mathf.Lerp(_mainCam.fieldOfView, targetValue, value);

            if (value > 0.9f)
            {
                break;
            }

            yield return null;
        }

        _mainCam.fieldOfView = targetValue;
        aimChangeCor = null;
    }

    #endregion
}
