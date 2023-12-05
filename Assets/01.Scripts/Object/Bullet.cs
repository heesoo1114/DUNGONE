using System.Collections;
using UnityEngine;

public class Bullet : PoolableMono
{
    [SerializeField] private float destroyDelayTime;
    
    public void SetPositionAndRotation(Transform tr)
    {
        transform.position = tr.position;
        transform.rotation = tr.rotation;
    }

    private IEnumerator DestroyCor()
    {
        yield return new WaitForSeconds(destroyDelayTime);
        PoolManager.Instance.Push(this);
    }

    public override void Init()
    {
        StartCoroutine(DestroyCor());
    }
}
