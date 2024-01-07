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
        // �ִϸ��̼�
        _animator.SetBool(isAttackHash, true);

        // ������ ó��
        Transform target = _stateMachineController.Target;
        if (target.TryGetComponent(out IDamageable damageableCmp))
        {
            damageableCmp.OnDamage(attackDamage);
        }

        // ��Ÿ��
        _stateMachineController.ActionAfterCoolTime(
            () => _stateMachine.ChangeState<StateIdle>(), 
            attackCoolTime);
    }

    public override void OnExit()
    {
        _animator?.SetBool(isAttackHash, false);
    }
}
