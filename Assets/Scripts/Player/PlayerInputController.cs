using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputController : MonoBehaviour
{
    public Vector2 MovementInputVector { get; private set; }
    public Vector2 LookInputVector { get; private set; }
    public bool IsMouseLook { get; private set; }
    public bool DanceTriggered { get; private set; }
    public bool AttackTriggered { get; private set; }

    [SerializeField]
    private GameObject capotePrefab;

    [SerializeField]
    private string mouseSchemeName = "Keyboard&Mouse";

    private SwitchCamera switchCamera;
    private SoundEffectPlayer soundEffectPlayer;
    private PlayerInput playerInput;
    private PlayerController playerController;

    private void Awake()
    {
        soundEffectPlayer = FindAnyObjectByType<SoundEffectPlayer>();
        switchCamera = GetComponent<SwitchCamera>();
        playerInput = GetComponent<PlayerInput>();
        playerController = GetComponent<PlayerController>();
    }

    private void OnMove(InputValue inputValue)
    {
        MovementInputVector = inputValue.Get<Vector2>();
        DanceTriggered = false;
    }

    private void OnLook(InputValue inputValue)
    {
        LookInputVector = inputValue.Get<Vector2>();
        IsMouseLook = playerInput.currentControlScheme == mouseSchemeName;
    }

    private void OnAttack(InputValue inputValue)
    {
        if (inputValue.isPressed)
        {
            AttackTriggered = true;

            GameObject projectileObj = Instantiate(capotePrefab);
            Capote projectile = projectileObj.GetComponent<Capote>();

            projectile.Init(
                transform.position + transform.forward,
                transform.forward,
                playerController.projectileDamage * playerController.damageMultiplier
            );

            if (soundEffectPlayer != null)
            {
                soundEffectPlayer.PlayThrowSound();
            }

            Destroy(projectileObj, 15f);
        }
    }

    private void OnDance(InputValue inputValue)
    {
        if (inputValue.isPressed && MovementInputVector.magnitude <= 0.01f)
        {
            DanceTriggered = true;
        }
    }

    public void ResetAttackTrigger()
    {
        AttackTriggered = false;
    }

    public void ResetDanceTrigger()
    {
        DanceTriggered = false;
    }

    private void OnSwitchCam(InputValue inputValue)
    {
        if (inputValue.isPressed)
        {
            switchCamera.ManageCamera();
        }
    }
}