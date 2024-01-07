using System.Collections;
using UnityEngine;
using System;

public class MonsterController : MonoBehaviour, IDamageable
{
    private StateMachine<MonsterController> thisStateMachine;

    private Coroutine runningDelayCoroutine = null;

    #region Health

    [Header("Health")]
    [SerializeField] private int maxHealth = 100;
    private int currentHealth;
    public int CurrentHealth => currentHealth;
    public bool IsAlive { get; private set; }

    #endregion

    #region Dissolve

    [Header("Effect")]
    [SerializeField] private float dissolveAnimationSpeed = 2f;
    private Material dissolveMaterial;
    private float offDissolveValue = 0;
    private float onDissolveValue = 1;

    #endregion

    private void Awake()
    {
        var renderer = GetComponentsInChildren<Renderer>();
        dissolveMaterial = renderer[0].material;
    }

    private void Start()
    {
        thisStateMachine = new StateMachine<MonsterController>(this, new StateIdle());
        thisStateMachine.AddStateList(new StateMove());
        thisStateMachine.AddStateList(new StateAttack());
        thisStateMachine.AddStateList(new StateHurt());
        thisStateMachine.AddStateList(new StateDie());

        IsAlive = true;
        currentHealth = maxHealth;
    }

    private void Update()
    {
        if (IsAlive)
        {
            thisStateMachine.Update(Time.deltaTime);
        }
    }

    // damage
    public void OnDamage(int damage)
    {
        currentHealth = Mathf.Clamp(currentHealth - damage, 0, maxHealth);
        // ui update

        if (currentHealth <= 0)
        {
            // die 
            thisStateMachine.ChangeState<StateDie>();
            IsAlive = false;
        }
        else
        {
            // hurt
            thisStateMachine.ChangeState<StateHurt>();
        }
    }

    public void OnDie()
    {
        StartCoroutine(DissolveCor(onDissolveValue, dissolveAnimationSpeed));
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

    public void ActionAfterCoolTime(Action action, float coolTime)
    {
        if (runningDelayCoroutine != null)
        {
            StopCoroutine(runningDelayCoroutine); 
        }
        runningDelayCoroutine = StartCoroutine(CoolTimeCor(action, coolTime));
    }

    private IEnumerator CoolTimeCor(Action action, float coolTime)
    {
        yield return new WaitForSeconds(coolTime);
        runningDelayCoroutine = null;
        action();
    }

    private IEnumerator DissolveCor(float targetValue, float animSpeed)
    {
        float moveTime = 0f;
        float timeValue = 0f;
        float dissolveValue = 0f;
        float endTimeValue = 1f / animSpeed;

        while (true)
        {
            moveTime += Time.deltaTime;
            timeValue = moveTime / animSpeed;

            dissolveValue = Mathf.SmoothStep(dissolveValue, targetValue, timeValue);
            dissolveMaterial.SetFloat("_Dissolve", dissolveValue);
            
            if (timeValue > endTimeValue)
            {
                break;
            }

            yield return null;
        }

        dissolveMaterial.SetFloat("_Dissolve", targetValue);
        Destroy(gameObject);
    }

    #endregion              
}
