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
    private GameObject pillowBarrierPrefab;

    [SerializeField]
    private string mouseSchemeName = "Keyboard&Mouse";

    private SwitchCamera switchCamera;
    private SoundEffectPlayer soundEffectPlayer;
    private PlayerInput playerInput;
    private PlayerController playerController;
    private NukeCinematicManager nukeCinematicManager;
    private Inventory inventory;

    [Header("Nuke Cooldown")]
    [SerializeField]
    private float nukeCooldownDuration = 60f;

    private float nextNukeAvailableTime = 0f;

    public float NukeCooldownRemaining =>
        Mathf.Max(0f, nextNukeAvailableTime - Time.unscaledTime);

    public bool IsNukeOnCooldown => NukeCooldownRemaining > 0f;

    private void Awake()
    {
        soundEffectPlayer = FindAnyObjectByType<SoundEffectPlayer>();
        switchCamera = GetComponent<SwitchCamera>();
        playerInput = GetComponent<PlayerInput>();
        playerController = GetComponent<PlayerController>();
        nukeCinematicManager = FindAnyObjectByType<NukeCinematicManager>();
        inventory = GetComponent<Inventory>();

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
                soundEffectPlayer.PlaySound(SoundEffectType.Throw);
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

    private void OnNuke(InputValue inputValue)
    {
        if (!inputValue.isPressed)
        {
            return;
        }

        if (inventory == null || !inventory.HasItem("Nuke"))
        {
            return;
        }

        if (IsNukeOnCooldown)
        {
            soundEffectPlayer.PlaySound(SoundEffectType.Error);
            return;
        }

        inventory.RemoveItemFromInventory("Nuke");
        nextNukeAvailableTime = Time.unscaledTime + nukeCooldownDuration;

        if (soundEffectPlayer != null)
        {
            soundEffectPlayer.PlaySound(SoundEffectType.NukeIncoming);
        }

        if (nukeCinematicManager != null)
        {
            nukeCinematicManager.PlayNukeCinematic();
        }
    }

    private void OnPlaceBarrier(InputValue inputValue)
    {
        if (inventory == null || !inventory.HasItem("BodyPillow"))
        {
            return;
        }
        inventory.RemoveItemFromInventory("BodyPillow");
        Instantiate(pillowBarrierPrefab, transform.position, transform.rotation);

        if (soundEffectPlayer != null)
        {
            soundEffectPlayer.PlaySound(SoundEffectType.Yamete);
        }
    }
}