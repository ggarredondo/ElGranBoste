using System;

public abstract class EnemyState
{
    protected string stateName;
    [NonSerialized] protected EnemyStateMachine enemy;
    public event Action OnEnter, OnExit;

    protected void Initialize(in string stateName, in EnemyStateMachine enemy)
    {
        this.stateName = stateName;
        this.enemy = enemy;
    }

    public virtual void Enter() => OnEnter?.Invoke();
    public abstract void Update();
    public virtual void Exit() => OnExit?.Invoke();

    public ref readonly string StateName => ref stateName;
}