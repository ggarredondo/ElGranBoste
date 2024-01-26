using System;

public abstract class PlayerState
{
    protected string stateName;
    [NonSerialized] protected PlayerStateMachine player;
    public event Action OnEnter, OnExit;

    protected void Initialize(in string stateName, in PlayerStateMachine player)
    {
        this.stateName = stateName;
        this.player = player;
    }

    public virtual void Enter() => OnEnter?.Invoke();
    public abstract void Update();
    public virtual void Exit() => OnExit?.Invoke();

    public ref readonly string StateName => ref stateName;
}