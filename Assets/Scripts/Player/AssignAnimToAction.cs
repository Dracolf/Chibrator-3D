using UnityEngine;

public class AssignAnimToAction : MonoBehaviour
{
    private Animator characterAnimator;
    private PlayerInputController playerInputController;

    private void Awake()
    {
        characterAnimator = GetComponentInChildren<Animator>();
        playerInputController = GetComponent<PlayerInputController>();
    }

    private void Update()
    {
        Vector2 move = playerInputController.MovementInputVector;

        characterAnimator.SetBool("IsMoving", move.magnitude > 0.01f);
        characterAnimator.SetFloat("MoveY", move.y);
        characterAnimator.SetFloat("MoveX", move.x);

        if (playerInputController.DanceTriggered)
        {
            characterAnimator.SetTrigger("DanceTrigger");
            playerInputController.ResetDanceTrigger();
        }
    }
}