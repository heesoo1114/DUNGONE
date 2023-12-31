using UnityEngine.AI;
using UnityEngine;

public class StateIdle : State<MonsterController>
{
    public override void OnAwake()
    {
        _characterController = _stateMachineController.GetComponent<CharacterController>();
    }

    public override void OnEnter()
    {
        Debug.Log("Enter StateIdle");

        // 현재 위치에서 정지 (만약에 안 되면 현재 위치를 SetDestination()에 인자로 보내주자)
        _characterController.Move(Vector3.zero);
    }

    public override void OnUpdate(float deltaTime)
    {
        Transform target = _stateMachineController.SearchEnemy();
        if (target)
        {
            if (_stateMachineController.GetFlagAttack)
            {
                _stateMachine.ChangeState<StateAttack>();
            }
            else
            {
                _stateMachine.ChangeState<StateMove>();
            }
        }
    }
}
