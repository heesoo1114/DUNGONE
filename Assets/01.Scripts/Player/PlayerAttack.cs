using System.Collections;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    private PlayerAnimator _anim;
    private CameraController _camController;

    // RayCasting
    private LayerMask hitableLayer;
    private LayerMask enemyLayer;
    private float maxDistance = float.PositiveInfinity;

    [SerializeField] private Transform muzzleTr;
    [SerializeField] private int sendDamage;

    [Header("Recoil")]
    [SerializeField] private float shakeMagnitude = 10f;
    [SerializeField] private float shakeDuration = 0.3f;

    #region Ammo

    [Header("Ammo")]
    [SerializeField] private int maxAmmo;
    [SerializeField] private float reloadDealtyTime = 2.3f;

    private int currentAmmo;
    public int CurrentAmmo
    {
        get => currentAmmo;
        set 
        {
            currentAmmo = value;
            // TODO: UI UPDATE
        }
    }
    private bool isReloading;

    #endregion

    public bool CanShoot() => (currentAmmo > 0) & (!isReloading);

    private void Awake()
    {
        _anim = GetComponent<PlayerAnimator>();
        _camController = Camera.main.GetComponent<CameraController>();

        hitableLayer = LayerMask.GetMask("Enemy", "GroundAndWall");
        enemyLayer = LayerMask.GetMask("Enemy");
    }

    private void Start()
    {
        InitAmmo();
    }

    public void InitAmmo()
    {
        currentAmmo = maxAmmo;
    }

    public void UseAmmo()
    {
        currentAmmo--;
    }

    #region Shoot

    // 발사를 시도하는 함수
    public void TryShoot()
    {
        // 총알이 있는지 체크
        if (!CanShoot())
        {
            Debug.Log("Not Enough Ammo");
            return;
        }

        ShootFeedback();
        UseAmmo();

        Ray ray = new Ray(muzzleTr.position, muzzleTr.transform.forward);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, maxDistance, hitableLayer))
        {
            if (hit.collider.includeLayers.Equals(enemyLayer))
            {
                SucessfullHit(hit);
            }
            else
            {
                FailedHit(hit);
            }
        }
    }

    private void ShootFeedback()
    {
        // Animation
        _anim.PlayAnimation("fire");
        StartCoroutine(DelayCor());

        // FireEffect
        Bullet bulletClone = PoolManager.Instance.Pop("Bullet") as Bullet;
        bulletClone.SetPositionAndRotation(muzzleTr);

        // Recoil (반동)
        _camController.ApplyRecoil(shakeMagnitude, shakeDuration);
    }

    #endregion

    #region HitFunc

    // 적에게 맞았을 때 실행되는 함수
    private void SucessfullHit(RaycastHit hit)
    {
        // 적 피격 시 피 튀기는 이펙트 재생
        HitEffect bloodEffectClone = PoolManager.Instance.Pop("BloodEffect") as HitEffect;
        bloodEffectClone.SetPositionAndRotation(hit.point, hit.normal);

        // hit에 저장된 피격 객체에 있는 IDamageAble 인터페이스 컴포넌트 가져와서 데미지 처리
        if (hit.collider.gameObject.TryGetComponent(out IDamageable damageableCmp))
        {
            damageableCmp.OnDamage(sendDamage);
        }
    }

    // 적이 아닌 땅, 벽에 맞았을 때 실행되는 함수
    private void FailedHit(RaycastHit hit)
    {
        // 충돌 시 총알 자국 이펙트 재생
        HitEffect conEffectClone = PoolManager.Instance.Pop("ConEffect") as HitEffect;
        conEffectClone.SetPositionAndRotation(hit.point, hit.normal);
    }

    #endregion

    #region Reload

    public void TryReload()
    {
        if (currentAmmo == maxAmmo)
        {
            // TODO: Ammo 없는 애니메이션이나 사운드 재생
            Debug.Log("Ammo is Enough");
            return;
        }

        StartCoroutine(ReloadCor());
    }

    private IEnumerator ReloadCor()
    {
        isReloading = true;

        _anim.PlayAnimation("reload");
        yield return new WaitForSeconds(reloadDealtyTime);
        _anim.StopAnimation("reload");
        
        currentAmmo = maxAmmo;
        isReloading = false;
    }

    #endregion

    private IEnumerator DelayCor()
    {
        yield return new WaitForSeconds(0.25f);
        _anim.StopAnimation("fire");
    }
}
