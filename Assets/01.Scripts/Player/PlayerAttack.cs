using System.Collections;
using UnityEngine;
using TMPro;

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

    [Header("Ammo")]
    [SerializeField] private int maxAmmo;
    [SerializeField] private float reloadDealtyTime = 2.3f;
    [SerializeField] private TextMeshProUGUI _currentAmmoText;
    private int currentAmmo;
    private bool isReloading;

    public bool IsCanShoot() => (currentAmmo > 0) & (!isReloading);

    private void Awake()
    {
        _anim = GetComponent<PlayerAnimator>();
        _camController = Camera.main.GetComponent<CameraController>();

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

    // �߻縦 �õ��ϴ� �Լ�
    public void TryShoot()
    {
        // �Ѿ��� �ִ��� üũ
        if (!IsCanShoot())
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

        // Recoil (�ݵ�)
        _camController.ApplyRecoil(shakeMagnitude, shakeDuration);
    }

    #endregion

    #region HitFunc

    // ������ �¾��� �� ����Ǵ� �Լ�
    private void SucessfullHit(RaycastHit hit)
    {
        // �� �ǰ� �� �� Ƣ��� ����Ʈ ���
        HitEffect bloodEffectClone = PoolManager.Instance.Pop("BloodEffect") as HitEffect;
        bloodEffectClone.SetPositionAndRotation(hit.point, hit.normal);

        // hit�� ����� �ǰ� ��ü�� �ִ� IDamageAble �������̽� ������Ʈ �����ͼ� ������ ó��
        if (hit.collider.gameObject.TryGetComponent(out IDamageable damageableCmp))
        {
            damageableCmp.OnDamage(sendDamage);
        }
    }

    // ���� �ƴ� ��, ���� �¾��� �� ����Ǵ� �Լ�
    private void FailedHit(RaycastHit hit)
    {
        // �浹 �� �Ѿ� �ڱ� ����Ʈ ���
        HitEffect conEffectClone = PoolManager.Instance.Pop("ConEffect") as HitEffect;
        conEffectClone.SetPositionAndRotation(hit.point, hit.normal);
    }

    #endregion

    #region Reload

    public void TryReload()
    {
        if (currentAmmo == maxAmmo)
        {
            // TODO: Ammo ���� �ִϸ��̼��̳� ���� ���
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

        UpdateAmmo(maxAmmo);
        isReloading = false;
    }

    #endregion

    private IEnumerator DelayCor()
    {
        yield return new WaitForSeconds(0.25f);
        _anim.StopAnimation("fire");
    }
}
