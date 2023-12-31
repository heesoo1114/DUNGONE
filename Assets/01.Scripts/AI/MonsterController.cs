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

    #endregion

    private void Start()
    {
        thisStateMachine = new StateMachine<MonsterController>(this, new StateIdle());
        thisStateMachine.AddStateList(new StateMove());
        thisStateMachine.AddStateList(new StateAttack());

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
    
    public void ActionLaterCoolTime(Action action, float coolTime)
    {
        StartCoroutine(CoolTimeCor(action, coolTime));
    }

    private IEnumerator CoolTimeCor(Action action, float coolTime)
    {
        yield return new WaitForSeconds(coolTime);
        action();
    }

    public void OnDamage(int damage)
    {
        currentHealth -= damage;
        // ui update
        if (currentHealth <= 0)
        {
            Debug.Log("Die");
        }
    }
}
