using UnityEngine;

public class UILookAtCamera : MonoBehaviour
{
    [SerializeField] private bool invert;

    private Vector3 camDir;
    private Transform camTransform;

    private void Awake()
    {
        camTransform = Camera.main.transform;
    }

    private void OnEnable()
    {
        LookCamera();
    }

    private void Update()
    {
        LookCamera();
    }

    private void LookCamera()
    {
        if (invert)
        {
            camDir = transform.position - camTransform.position;
            transform.LookAt(transform.position + camDir.normalized);
        }
        else
        {
            transform.LookAt(camTransform);
        }
    }
}
