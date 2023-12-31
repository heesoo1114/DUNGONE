using System.Collections;
using UnityEngine;
using TMPro;

public class PlayerAttack : MonoBehaviour
{
    #region Property

    private PlayerController _playerController;
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

    [Header("Ammo")]
    [SerializeField] private int maxAmmo;
    [SerializeField] private float reloadDealtyTime = 2.3f;
    [SerializeField] private TextMeshProUGUI _currentAmmoText;
    private int currentAmmo;
    private bool isReloading;
    public bool CanReload()
    {
        return (currentAmmo != maxAmmo) && (false == isReloading) && (false == _playerController.IsMoving);
    }

    [Header("Sound")]
    [SerializeField] private AudioClip shootSound;
    [SerializeField] private AudioClip reloadSound;
    [SerializeField] private AudioClip notAmmoSound;
    [SerializeField] private AudioClip sucessfulHitSound;
    [SerializeField] private AudioClip failHitSound;

    // bool
    public bool IsShooting { get; private set; }
    public bool CanShoot()
    {
        // print(_playerController.IsAiming + " " + _playerController.IsMoving);
        if (false == _playerController.IsAiming && true == _playerController.IsMoving)
        {
            return false;   
        }
        return (currentAmmo > 0) & (false == isReloading);
    }

    #endregion

    private void Awake()
    {
        _anim = GetComponent<PlayerAnimator>();
        _camController = Camera.main.GetComponent<CameraController>();
        _playerController = GetComponent<PlayerController>();

        hitableLayer = LayerMask.GetMask("Enemy", "GroundAndWall");
        enemyLayer = LayerMask.GetMask("Enemy");
    }

    private void Start()
    {
        UpdateAmmo(maxAmmo);
    }

    #region Ammo

    private void UpdateAmmo(int newAmmo)
    {
        currentAmmo = newAmmo;
        _currentAmmoText.text = currentAmmo.ToString() + " / " + maxAmmo;
    }

    public void UseAmmo()
    {
        UpdateAmmo(--currentAmmo);
    }

    #endregion

    #region Shoot

    // 발사를 시도하는 함수
    public void TryShoot()
    {
        // 총알이 있는지 체크
        if (false == CanShoot())
        {
            MakeSound(notAmmoSound);
            return;
        }

        IsShooting = true;

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

        IsShooting = false;
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

        // Sound
        MakeSound(shootSound);
    }

    #endregion

    #region HitFunc

    // 적에게 맞았을 때 실행되는 함수
    private void SucessfullHit(RaycastHit hit)
    {
        // 적 피격 시 피 튀기는 이펙트 재생
        HitEffect bloodEffectClone = PoolManager.Instance.Pop("BloodEffect") as HitEffect;
        bloodEffectClone.SetPositionAndRotation(hit.point, hit.normal);

        // 소리 재생
        MakeSound(sucessfulHitSound);

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

        // 소리 재생
        MakeSound(failHitSound);
    }

    #endregion

    #region Reload

    public void TryReload()
    {
        if (false == CanReload())
        {
            return;
        }

        StartCoroutine(ReloadCor());
    }

    private IEnumerator ReloadCor()
    {
        isReloading = true;

        MakeSound(reloadSound);

        _anim.PlayAnimation("reload");
        yield return new WaitForSeconds(reloadDealtyTime);
        _anim.StopAnimation("reload");

        UpdateAmmo(maxAmmo);
        isReloading = false;
    }

    #endregion

    private void MakeSound(AudioClip clip)
    {
        AudioObj audioObjClone = PoolManager.Instance.Pop("AudioObj") as AudioObj;
        audioObjClone.PlayClip(clip);
    }

    private IEnumerator DelayCor()
    {
        yield return new WaitForSeconds(0.25f);
        _anim.StopAnimation("fire");
    }
}
