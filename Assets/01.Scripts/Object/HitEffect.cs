using System.Collections;
using UnityEngine;

public class HitEffect : PoolableMono
{
    [SerializeField] private float destroyDelayTime;

    public void SetPositionAndRotation(Vector3 pos, Vector3 dir)
    {
        transform.position = pos + (dir * 0.1f);
        transform.rotation = Quaternion.FromToRotation(Vector3.forward, dir);
        
        // transform.rotation = Quaternion.Euler(dir.x, dir.y, 90);
        // 위 FromToRotation 함수 사용한 회전과 같은 역할

        // 로테이션이 Z축으로 90도를 하는 것이 포인트였음
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
