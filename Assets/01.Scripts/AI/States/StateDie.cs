using UnityEngine.AI;
using UnityEngine;

public class StateDie : State<MonsterController>
{
    private int isDie = Animator.StringToHash("isDie");

    public override void OnAwake()
    {
        _characterController = _stateMachineController.GetComponent<CharacterController>();
        _navMeshAgent = _stateMachineController.GetComponent<NavMeshAgent>();
        _animator = _stateMachineController.GetComponentInChildren<Animator>();
    }

    public override void OnEnter()
    {
        Debug.Log("<color=red>Enter DieState</color>");

        _navMeshAgent.velocity = Vector3.zero;
        _navMeshAgent.isStopped = true;
        _characterController.Move(Vector3.zero);
        _animator?.SetTrigger(isDie);
    }

    public override void OnUpdate(float deltaTime)
    {

    }

    public override void OnExit()
    {
        
    }
}
