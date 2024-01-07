using UnityEngine.AI;
using UnityEngine;

public class StateHurt : State<MonsterController>
{
    private int isHurt = Animator.StringToHash("isHurt");
    private float hurtAnimationClipLength;

    public override void OnAwake()
    {
        _characterController = _stateMachineController.GetComponent<CharacterController>();
        _navMeshAgent = _stateMachineController.GetComponent<NavMeshAgent>();
        _animator = _stateMachineController.GetComponentInChildren<Animator>();
        
        hurtAnimationClipLength = _stateMachineController.GetAnimationClipLength(_animator, "Damage");
    }

    public override void OnEnter()
    {
        Debug.Log("<color=#FAD656>Enter HurtState</color>");

        _navMeshAgent.velocity = Vector3.zero;
        _characterController.Move(Vector3.zero);

        _animator?.SetBool(isHurt, true);
        _stateMachineController.ActionAfterCoolTime(                        
            () => _stateMachine.ChangeState<StateIdle>(),
            hurtAnimationClipLength);
    }                                                                            

    public override void OnUpdate(float deltaTime)
    {
        
    }

    public override void OnExit()
    {
        _animator.SetBool(isHurt, false);
    }
}
