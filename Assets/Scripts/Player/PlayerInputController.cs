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

    private bool IsGamePaused()
    {
        return PauseManager.Instance != null && PauseManager.Instance.IsPaused;
    }

    private void OnMove(InputValue inputValue)
    {
        if (IsGamePaused())
        {
            MovementInputVector = Vector2.zero;
            DanceTriggered = false;
            return;
        }

        MovementInputVector = inputValue.Get<Vector2>();
        DanceTriggered = false;
    }

    private void OnLook(InputValue inputValue)
    {
        if (IsGamePaused())
        {
            LookInputVector = Vector2.zero;
            return;
        }

        LookInputVector = inputValue.Get<Vector2>();

        if (playerInput != null)
        {
            IsMouseLook = playerInput.currentControlScheme == mouseSchemeName;
        }
    }

    private void OnAttack(InputValue inputValue)
    {
        if (IsGamePaused())
        {
            return;
        }

        if (!inputValue.isPressed)
        {
            return;
        }

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

    private void OnDance(InputValue inputValue)
    {
        if (IsGamePaused())
        {
            return;
        }

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
        if (IsGamePaused())
        {
            return;
        }

        if (inputValue.isPressed && switchCamera != null)
        {
            switchCamera.ManageCamera();
        }
    }

    private void OnNuke(InputValue inputValue)
    {
        if (IsGamePaused())
        {
            return;
        }

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
            if (soundEffectPlayer != null)
            {
                soundEffectPlayer.PlaySound(SoundEffectType.Error);
            }

            return;
        }

        inventory.RemoveItemFromInventory("Nuke");
        nextNukeAvailableTime = Time.unscaledTime + nukeCooldownDuration;
        int nbNukeUses = PlayerPrefs.GetInt("nukesUsed");
        PlayerPrefs.SetInt("nukesUsed", nbNukeUses + 1);

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
        if (IsGamePaused())
        {
            return;
        }

        if (!inputValue.isPressed)
        {
            return;
        }

        if (inventory == null || !inventory.HasItem("BodyPillow"))
        {
            return;
        }

        inventory.RemoveItemFromInventory("BodyPillow");
        int nbPillowUses = PlayerPrefs.GetInt("pillowsUsed");
        PlayerPrefs.SetInt("pillowsUsed", nbPillowUses + 1);

        Instantiate(pillowBarrierPrefab, transform.position, transform.rotation);

        if (soundEffectPlayer != null)
        {
            soundEffectPlayer.PlaySound(SoundEffectType.Yamete);
        }
    }

    private void OnPause(InputValue inputValue)
    {
        if (!inputValue.isPressed)
        {
            return;
        }

        bool openedWithGamepad = playerInput != null && playerInput.currentControlScheme == "Gamepad";

        if (PauseManager.Instance != null)
        {
            PauseManager.Instance.BackToPauseMenu();
            PauseManager.Instance.TogglePause(openedWithGamepad);
        }
    }
}