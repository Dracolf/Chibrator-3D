using UnityEngine;
using UnityEngine.InputSystem;

public class AssignAnimToAction : MonoBehaviour
{
    private Animator characterAnimator;

    private void Start()
    {
        characterAnimator = GetComponentInChildren<Animator>();
    }

    private void OnMove(InputValue inputValue)
    {
        Vector2 move = inputValue.Get<Vector2>();

        characterAnimator.SetBool("IsMoving", move.magnitude > 0.01f);
        characterAnimator.SetFloat("MoveY", move.y);
        characterAnimator.SetFloat("MoveX", move.x);
    }
}