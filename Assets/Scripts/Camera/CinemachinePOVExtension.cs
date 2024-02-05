using UnityEngine;
using Cinemachine;
using UnityEngine.InputSystem;
using DG.Tweening;
using System.Collections.Generic;

public class CinemachinePOVExtension : CinemachineExtension
{
    [Header("Player")]
    [SerializeField] private string playerTag;

    [Header("Input")]
    [SerializeField] private InputActionReference look;

    [Header("Events")]
    [SerializeField] private EventHandler eventHandler;
    [Space(10)]
    [SerializeField] private string deathEventName;
    [SerializeField] private string parryEventName;

    [Header("Look Parameters")]
    [SerializeField] private float horizontalSpeed = 10f;
    [SerializeField] private float verticalSpeed = 10f;
    [SerializeField] private float clampYAngleDown = -80f;
    [SerializeField] private float clampYAngleUp = 80f;

    [Header("Walking Camera Shake")]
    [SerializeField] private float amplitudeY = 0.1f;
    [SerializeField] private float amplitudeX = 0.1f;
    [SerializeField] private float shakeFrequency = 0.5f;

    [Header("Death Camera Movement")]
    [SerializeField] private Vector3 deathPosition;
    [SerializeField] private Vector3 deathRotation;
    [SerializeField] private float deathMovementTime;

    [Header("Parry Camera Shake")]
    [SerializeField] private float parryShakeTime;
    [SerializeField] private Vector3 parryShakeStrength;
    [SerializeField] private int parryShakeVibrato = 10;
    [SerializeField] private float parryShakeRandom = 90;

    [Header("Sounds")]
    [SerializeField] private string stepSoundName;

    private Vector2 mouseInput;
    private Vector3 startingRotation;
    private Vector3 initialPosition;
    private Transform target;
    private PlayerStateMachine player;

    private Sequence sequenceY, sequenceX;
    private Sequence deathSequence;
    private Sequence parrySequence;

    protected override void Awake()
    {
        look.action.performed += ctx => mouseInput = ctx.ReadValue<Vector2>();
        target = VirtualCamera.Follow;
        initialPosition = target.localPosition;

        horizontalSpeed = GameManager.Save.Options.mouseSensitivity;
        verticalSpeed = GameManager.Save.Options.mouseSensitivity;

        base.Awake();
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();

        sequenceX.Kill();
        sequenceY.Kill();
        deathSequence.Kill();
        parrySequence.Kill();
    }

    private void Start()
    {
        PlayerCameraShake();

        eventHandler.events[deathEventName] += DeathCameraMovement;
        eventHandler.events[parryEventName] += ParryCameraShake;

        player = GameObject.FindGameObjectWithTag(playerTag).GetComponent<PlayerStateMachine>();
    }

    private void Update()
    {
        if (!PauseController.pauseMenuActivated)
            UpdateDuration();
        else
        {
            horizontalSpeed = GameManager.Save.Options.mouseSensitivity;
            verticalSpeed = GameManager.Save.Options.mouseSensitivity;
        }
    }

    private void UpdateDuration()
    {
        float velocity = player.Velocity.magnitude / player.RunningState.MovementSpeed;

        sequenceX.DOTimeScale(velocity, 0f);
        sequenceY.DOTimeScale(velocity, 0f);
    }

    private void ParryCameraShake()
    {
        sequenceX.Pause();
        sequenceY.Pause();

        parrySequence = DOTween.Sequence();
        parrySequence.SetUpdate(true);
        parrySequence.Append(target.DOShakePosition(parryShakeTime, parryShakeStrength, parryShakeVibrato, parryShakeRandom, randomnessMode: ShakeRandomnessMode.Harmonic));
        parrySequence.Append(target.DOLocalMove(initialPosition, 0.1f)).OnComplete(() =>
        {
            sequenceX.Play();
            sequenceY.Play();
        });
    }

    private void DeathCameraMovement()
    {
        sequenceX.Kill();
        sequenceY.Kill();

        horizontalSpeed = 0;
        verticalSpeed = 0;

        deathSequence = DOTween.Sequence();
        deathSequence.Append(target.DOLocalMove(deathPosition, deathMovementTime));
        deathSequence.Join(target.DOLocalRotate(deathRotation, deathMovementTime)).OnComplete(ResetScene);

        eventHandler.events[deathEventName] -= DeathCameraMovement;
    }

    private void ResetScene()
    {
        GameManager.Scene.PreviousScene();
    }

    private void PlayerCameraShake()
    {
        sequenceY = DOTween.Sequence();
        sequenceX = DOTween.Sequence();

        sequenceY.Append(target.DOLocalMoveY(amplitudeY, shakeFrequency).SetEase(Ease.InOutSine));
        sequenceY.SetLoops(-1, LoopType.Yoyo);

        sequenceX.Append(target.DOLocalMoveX(amplitudeX, shakeFrequency * 2).From(-amplitudeX).SetEase(Ease.InOutSine));
        sequenceX.SetLoops(-1, LoopType.Yoyo).OnStepComplete(PlaySound);
    }

    private void PlaySound()
    {
        GameManager.Audio.Play(stepSoundName);
    }

    protected override void PostPipelineStageCallback(CinemachineVirtualCameraBase vcam, CinemachineCore.Stage stage, ref CameraState state, float deltaTime)
    {
        if (vcam.Follow)
        {
            if(stage == CinemachineCore.Stage.Aim)
            {
                if (startingRotation == null) startingRotation = transform.localRotation.eulerAngles;
                startingRotation.x += mouseInput.x * horizontalSpeed * Time.deltaTime;
                startingRotation.y += mouseInput.y * verticalSpeed * Time.deltaTime;
                startingRotation.y = Mathf.Clamp(startingRotation.y, clampYAngleDown, clampYAngleUp);
                state.RawOrientation = Quaternion.Euler(-startingRotation.y, startingRotation.x, 0f);
            }
        }
    }
}
