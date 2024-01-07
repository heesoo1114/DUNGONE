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

    #region Shake

    public void ApplyRecoil(float magnitude, float duration)
    {
        StartCoroutine(RecoilCor(magnitude, duration));
    }

    private IEnumerator RecoilCor(float angle, float duration)
    {
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            float progress = elapsedTime / duration;
            float recoilAngleThisFrame = Mathf.Lerp(0f, angle, progress);
            _mainCam.transform.localRotation = originalCameraRotation * Quaternion.Euler(-recoilAngleThisFrame, 0f, 0f);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        _mainCam.transform.localRotation = originalCameraRotation;
    }

    #endregion

    #region AimMode

    // Á¶ÁØ°æ
    public void ZoomIn()
    {
        IsAiming = true;

        if (aimChangeCor != null)
        {
            StopCoroutine(aimChangeCor);
        }
        aimChangeCor = StartCoroutine(AimChangeAnim(targetAimvalue, aimChangeAnimSpeed));
    }

    // °ßÂø
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
