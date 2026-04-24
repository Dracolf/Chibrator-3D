using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputController : MonoBehaviour
{
    public Vector2 MovementInputVector { get; private set; }
    public bool DanceTriggered { get; private set; }

    private SwitchCamera switchCamera;


    private void Awake()
    {
        switchCamera = GetComponent<SwitchCamera>();
    }
    private void OnMove(InputValue inputValue)
    {
        MovementInputVector = inputValue.Get<Vector2>();
        DanceTriggered = false;
    }

    private void OnDance(InputValue inputValue)
    {
        if (inputValue.isPressed && MovementInputVector.magnitude <= 0.01f)
        {
            DanceTriggered = true;
        }
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