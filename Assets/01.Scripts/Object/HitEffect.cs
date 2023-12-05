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
        // �� FromToRotation �Լ� ����� ȸ���� ���� ����

        // �����̼��� Z������ 90���� �ϴ� ���� ����Ʈ����
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
