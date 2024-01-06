using System.Collections;
using UnityEngine;
using System;

public class MonsterController : MonoBehaviour, IDamageable
{
    private StateMachine<MonsterController> thisStateMachine;

    #region Health

    [SerializeField] private int maxHealth = 100;
    private int currentHealth;
    public int CurrentHealth => currentHealth;
    public bool IsDead { get; private set; }

    #endregion

    private void Start()
    {
        thisStateMachine = new StateMachine<MonsterController>(this, new StateIdle());
        thisStateMachine.AddStateList(new StateMove());
        thisStateMachine.AddStateList(new StateAttack());
        thisStateMachine.AddStateList(new StateHurt());
        thisStateMachine.AddStateList(new StateDie());

        currentHealth = maxHealth;
    }

    #region Conditions

    [Header("Serching")]
    [SerializeField] private LayerMask targetLayerMask;
    [SerializeField] private float eyeSight;
    private Transform target;
    public Transform Target => target;
    public Transform SearchEnemy()
    {
        target = null;

        Collider[] findTargets = Physics.OverlapSphere(transform.position, eyeSight, targetLayerMask);

        if (findTargets.Length > 0)
        {
            target = findTargets[0].transform;
        }

        return target;
    }

    [Header("AttackValue")]
    [SerializeField] private int attackDamage = 5;
    public int AttackDamage => attackDamage;

    [SerializeField] private float attackCoolTime = 1.5f;
    public float AttackCoolTime => attackCoolTime;

    [SerializeField] private float attackRange;
    public bool GetFlagAttack
    {
        get
        {
            if (!target)
            {
                return false;
            }

            float distance = Vector3.Distance(transform.position, target.position);
            return (distance <= attackRange);
        }
    }

    #endregion

    private void Update()
    {
        thisStateMachine.Update(Time.deltaTime);
    }

    // damage
    public void OnDamage(int damage)
    {
        currentHealth -= damage;
        // ui update

        // hurt
        thisStateMachine.ChangeState<StateHurt>();   

        if (currentHealth <= 0)
        {
            IsDead = true;

            // die 
            thisStateMachine.ChangeState<StateDie>();
        }
    }

    #region Funcs

    public float GetAnimationClipLength(Animator _animator, string clipName)
    {
        float time = 0;

        AnimationClip[] clipList = _animator.runtimeAnimatorController.animationClips;
        AnimationClip clip = Array.Find(clipList, c => c.name == clipName);

        if (clip != null)
        {
            time = clip.length;
        }
        else
        {
            Debug.Log($"{clipName} animation clip is null");
        }

        return time;
    }

    public void ActionLaterCoolTime(Action action, float coolTime)
    {
        Debug.Log(coolTime);
        StopAllCoroutines();
        StartCoroutine(CoolTimeCor(action, coolTime));
    }

    private IEnumerator CoolTimeCor(Action action, float coolTime)
    {
        yield return new WaitForSeconds(coolTime);
        action();
    }

    #endregion              
}
