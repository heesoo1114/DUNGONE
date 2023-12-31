using UnityEngine.AI;
using UnityEngine;

public abstract class State<T>
{
    protected StateMachine<T> _stateMachine;
    protected T _stateMachineController;

    protected CharacterController _characterController;
    protected NavMeshAgent _navMeshAgent;
    protected Animator _animator;

    public virtual void OnAwake() { }
    public virtual void OnEnter() { }
    public abstract void OnUpdate(float deltaTime);
    public virtual void OnExit() { }

    public void SetMachineWithController(StateMachine<T> stateMachine, T stateMachineController)
    {
        this._stateMachine = stateMachine;
        this._stateMachineController = stateMachineController;
        OnAwake();
    }
}
