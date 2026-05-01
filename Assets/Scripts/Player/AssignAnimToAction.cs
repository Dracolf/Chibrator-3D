using UnityEngine;

public class AssignAnimToAction : MonoBehaviour
{
    private Animator characterAnimator;
    private PlayerInputController playerInputController;

    private const string IsMovingParam = "IsMoving";
    private const string MoveYParam = "MoveY";
    private const string MoveXParam = "MoveX";
    private const string ThrowTriggerParam = "ThrowTrigger";
    private const string DanceTriggerParam = "DanceTrigger";

    private void Awake()
    {
        characterAnimator = GetComponentInChildren<Animator>();
        playerInputController = GetComponent<PlayerInputController>();
    }

    private void Update()
    {
        Vector2 move = playerInputController.MovementInputVector;
        bool isMoving = move.magnitude > 0.01f;

        characterAnimator.SetBool(IsMovingParam, isMoving);
        characterAnimator.SetFloat(MoveYParam, move.y);
        characterAnimator.SetFloat(MoveXParam, move.x);

        if (isMoving)
        {
            characterAnimator.ResetTrigger(DanceTriggerParam);
            playerInputController.ResetDanceTrigger();
        }

        if (playerInputController.AttackTriggered)
        {
            characterAnimator.ResetTrigger(DanceTriggerParam);
            characterAnimator.ResetTrigger(ThrowTriggerParam);
            characterAnimator.SetTrigger(ThrowTriggerParam);

            playerInputController.ResetAttackTrigger();
            playerInputController.ResetDanceTrigger();

            return;
        }

        if (playerInputController.DanceTriggered && !isMoving)
        {
            characterAnimator.ResetTrigger(ThrowTriggerParam);
            characterAnimator.ResetTrigger(DanceTriggerParam);
            characterAnimator.SetTrigger(DanceTriggerParam);

            playerInputController.ResetDanceTrigger();
        }
    }
}