using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputController : MonoBehaviour
{
    public Vector2 MovementInputVector { get; private set; }
    public bool DanceTriggered { get; private set; }

    private void OnMove(InputValue inputValue)
    {
        MovementInputVector = inputValue.Get<Vector2>();
    }

    private void OnDance(InputValue inputValue)
    {
        if (inputValue.isPressed)
        {
            DanceTriggered = true;
        }
    }

    public void ResetDanceTrigger()
    {
        DanceTriggered = false;
    }
}