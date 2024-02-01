using DG.Tweening;
using UnityEngine;

[System.Serializable]
public class ParryState : PlayerState
{
    [SerializeField] private float movementSpeed = 1f;
    [SerializeField] private float startUp, active, recovery;
    private Sequence sequence;
    public void Initialize(in PlayerStateMachine player) => base.Initialize("PARRY", player);

    public override void Enter()
    {
        player.ResetMoveVector();
        sequence = DOTween.Sequence();
        sequence.AppendInterval(startUp);
        sequence.AppendCallback(() => {
            player.PlayerToEnemyEvents.OnKillPlayer += Parry;
        });
        sequence.AppendInterval(active);
        sequence.AppendCallback(() => {
            player.PlayerToEnemyEvents.OnKillPlayer -= Parry;
            player.PlayerToEnemyEvents.OnKillPlayer += player.TransitionToDead;
        });
        sequence.AppendInterval(recovery).OnComplete(player.TransitionToRunning);
        base.Enter();
    }
    public override void Update()
    {
        player.LookForward();
        player.Move(movementSpeed);
        player.Fall();
    }
    public override void Exit()
    {
        sequence.Kill();
        player.PlayerToEnemyEvents.OnParry = null;
        player.PlayerToEnemyEvents.OnKillPlayer -= player.TransitionToDead;
        base.Exit();
    }

    private void Parry()
    {
        player.PlayerToEnemyEvents.OnParry?.Invoke();
        player.TransitionToRunning();
    }

    public float StartUp => startUp;
    public float Active => active;
    public float Recovery => recovery;
}
