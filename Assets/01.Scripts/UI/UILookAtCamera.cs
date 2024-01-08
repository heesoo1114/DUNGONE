using UnityEngine;

public class UILookAtCamera : MonoBehaviour
{
    [SerializeField] private bool invert;

    private Vector3 camDir;
    private Vector3 camPos;
    private Transform camTransform;

    private void Awake()
    {
        camTransform = Camera.main.transform;
    }

    private void OnEnable()
    {
        if (invert)
        {
            transform.rotation = Quaternion.Euler(0, 0, 0);
        }
        LookCamera();
    }

    private void Update()
    {
        LookCamera();
    }

    private void LookCamera()
    {
        camDir = transform.position - camTransform.position;
        camDir.y = 0;
        transform.LookAt(transform.position + camDir.normalized);
    }
}
