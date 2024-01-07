using UnityEngine;

public class StateAttack : State<MonsterController> 
{
    private int isAttackHash = Animator.StringToHash("isAttack");

    private float attackCoolTime;
    private int attackDamage;

    public override void OnAwake()
    {
        _characterController = _stateMachineController.GetComponent<CharacterController>();
        _animator = _stateMachineController.GetComponentInChildren<Animator>();
        attackCoolTime = _stateMachineController.AttackCoolTime;
        attackDamage = _stateMachineController.AttackDamage;
    }

    public override void OnEnter()
    {
        Debug.Log("Enter StateAttack");

        Attack();
    }

    public override void OnUpdate(float deltaTime)
    {

    }

    private void Attack()
    {
        // 애니메이션
        _animator.SetBool(isAttackHash, true);

        // 데미지 처리
        Transform target = _stateMachineController.Target;
        if (target.TryGetComponent(out IDamageable damageableCmp))
        {
            damageableCmp.OnDamage(attackDamage);
        }

        // 쿨타임
        _stateMachineController.ActionAfterCoolTime(
            () => _stateMachine.ChangeState<StateIdle>(), 
            attackCoolTime);
    }

    public override void OnExit()
    {
        _animator?.SetBool(isAttackHash, false);
    }
}
