using UnityEngine;
using Cinemachine;
using UnityEngine.InputSystem;
using DG.Tweening;

public class CinemachinePOVExtension : CinemachineExtension
{
    [Header("Player")]
    [SerializeField] private string playerTag;

    [Header("Input")]
    [SerializeField] private InputActionReference look;

    [Header("Look Parameters")]
    [SerializeField] private float horizontalSpeed = 10f;
    [SerializeField] private float verticalSpeed = 10f;
    [SerializeField] private float clampYAngleDown = -80f;
    [SerializeField] private float clampYAngleUp = 80f;

    [Header("Walking Camera Shake")]
    [SerializeField] private float amplitudeY = 0.1f;
    [SerializeField] private float amplitudeX = 0.1f;
    [SerializeField] private float shakeFrequency = 0.5f;

    private Vector2 mouseInput;
    private Vector3 startingRotation;
    private Transform target;
    private PlayerStateMachine player;

    private Sequence sequenceY, sequenceX;

    protected override void Awake()
    {
        look.action.performed += ctx => mouseInput = ctx.ReadValue<Vector2>();
        target = VirtualCamera.Follow;

        sequenceY = DOTween.Sequence();
        sequenceX = DOTween.Sequence();

        CameraShake();

        base.Awake();
    }

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag(playerTag).GetComponent<PlayerStateMachine>();
    }

    private void Update()
    {
        UpdateDuration();
    }

    private void UpdateDuration()
    {
        sequenceX.DOTimeScale(player.Velocity.magnitude / 5, 0f);
        sequenceY.DOTimeScale(player.Velocity.magnitude / 5, 0f);
    }

    private void CameraShake()
    {
        sequenceY.Append(target.DOLocalMoveY(amplitudeY, shakeFrequency).SetEase(Ease.InOutSine));
        sequenceY.SetLoops(-1, LoopType.Yoyo);

        sequenceX.Append(target.DOLocalMoveX(amplitudeX, shakeFrequency * 2).From(-amplitudeX).SetEase(Ease.InOutSine));
        sequenceX.SetLoops(-1, LoopType.Yoyo);
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
