using UnityEngine.AI;
using UnityEngine;

public class StateMove : State<MonsterController>
{
    private int isMoveHash = Animator.StringToHash("isMove");

    public override void OnAwake()
    {
        _characterController = _stateMachineController.GetComponent<CharacterController>();
        _navMeshAgent = _stateMachineController.GetComponent<NavMeshAgent>();
        _animator = _stateMachineController.GetComponentInChildren<Animator>();
    }

    public override void OnEnter()
    {
        Debug.Log("Enter StateMove");

        _navMeshAgent?.SetDestination(_stateMachineController.Target.position); // 이미 idle에서 Target에 넣어줌
        _animator?.SetBool(isMoveHash, true);
    }

    public override void OnUpdate(float deltaTime)
    {
        Transform target = _stateMachineController.SearchEnemy();
        if (target != null)
        {
            _navMeshAgent.SetDestination(target.position);
        }

        // 도착하지 않았으면 계속 움직이고, 도착하였으면 idle로
        if (_navMeshAgent.remainingDistance > _navMeshAgent.stoppingDistance)
        {
            _characterController.Move(_navMeshAgent.velocity * deltaTime);
        }
        else
        {
            _stateMachine.ChangeState<StateIdle>();
        }
    }

    public override void OnExit()
    {
        _animator?.SetBool(isMoveHash, false);
        _navMeshAgent.ResetPath();
    }
}
