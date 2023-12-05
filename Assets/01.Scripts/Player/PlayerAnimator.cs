using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimator : MonoBehaviour
{
    [Header("Animator")]
    [SerializeField] private Animator _armAnim;
    [SerializeField] private Animator _weaponAnim;

    // blend
    [Range(0f, 1f)] public float startAnimTime = 0.3f;

    private readonly int _fireHash   = Animator.StringToHash("isFiring");    // (PlayerAttack.cs)
    private readonly int _reloadHash = Animator.StringToHash("isReloading"); //(PlayerAttack.cs)
    private readonly int _aimHash    = Animator.StringToHash("isAiming");    // _weaponAnim은 없음 (PlayerLook.cs)
    private readonly int _moveHash   = Animator.StringToHash("isMoving");    // _weaponAnim은 없음
    private readonly int _blendHash = Animator.StringToHash("blend");

    private Dictionary<string, int> armAnimHashList = new Dictionary<string, int>();
    private Dictionary<string, int> weaponAnimHashList = new Dictionary<string, int>();
    
    private void Start()
    {
        armAnimHashList.Add("fire", _fireHash);
        armAnimHashList.Add("reload",_reloadHash);
        armAnimHashList.Add("aim", _aimHash);
        armAnimHashList.Add("move", _moveHash); 
        
        weaponAnimHashList.Add("fire", _fireHash);
        weaponAnimHashList.Add("reload", _reloadHash);
    }

    public void PlayAnimation(string key)
    {
        int armHashKey = armAnimHashList.TryGetValue(key, out armHashKey) ? armHashKey : 0;
        int weaponHashKey = weaponAnimHashList.TryGetValue(key, out weaponHashKey) ? weaponHashKey : 0; 

        if (armHashKey != 0)
        {
            _armAnim.SetBool(armHashKey, true);   
        }

        if (weaponHashKey != 0)
        {
            _weaponAnim.SetBool(weaponHashKey, true);
        }
    }
    
    public void StopAnimation(string key)
    {
        int armHashKey = armAnimHashList.TryGetValue(key, out armHashKey) ? armHashKey : 0;
        int weaponHashKey = weaponAnimHashList.TryGetValue(key, out weaponHashKey) ? weaponHashKey : 0;

        if (armHashKey != 0)
        {
            _armAnim.SetBool(armHashKey, false);
        }

        if (weaponHashKey != 0)
        {
            _weaponAnim.SetBool(weaponHashKey, false);
        }
    }

    public void SetBlendValue(float blend)
    {
        _armAnim.SetFloat(_blendHash, blend, startAnimTime, Time.deltaTime);
    }
}
