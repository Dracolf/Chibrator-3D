using UnityEngine;

public class RastaCameraManager : MonoBehaviour
{
    public static RastaCameraManager Instance { get; private set; }

    [Header("References")]
    [SerializeField]
    private Transform playerTransform;

    [SerializeField]
    private Transform cameraPivot;

    [Header("Normal Cameras")]
    [SerializeField]
    private GameObject[] normalCameras;

    [Header("Rasta Camera")]
    [SerializeField]
    private GameObject rastaLockedCamera;

    [Header("Look Settings")]
    [SerializeField]
    private Vector3 rastaLookOffset = new Vector3(0f, 1.5f, 0f);

    [SerializeField]
    private float playerRotationSpeed = 12f;

    [SerializeField]
    private float cameraPitchSpeed = 12f;

    [SerializeField]
    private float minCameraPitch = -70f;

    [SerializeField]
    private float maxCameraPitch = 70f;

    private Transform currentRastaTarget;
    private GameObject previousActiveCamera;
    private bool isLockedOnRasta;

    public bool IsLockedOnRasta => isLockedOnRasta;

    private void Awake()
    {
        Instance = this;

        if (rastaLockedCamera != null)
        {
            rastaLockedCamera.SetActive(false);
        }
    }

    private void LateUpdate()
    {
        if (!isLockedOnRasta || currentRastaTarget == null)
        {
            return;
        }

        RotatePlayerTowardRasta();
        RotateCameraPivotTowardRasta();
    }

    public void LockOnRasta(Transform rastaTarget)
    {
        if (rastaTarget == null || rastaLockedCamera == null)
        {
            return;
        }

        currentRastaTarget = rastaTarget;
        isLockedOnRasta = true;

        previousActiveCamera = GetCurrentActiveNormalCamera();

        foreach (GameObject normalCamera in normalCameras)
        {
            if (normalCamera != null)
            {
                normalCamera.SetActive(false);
            }
        }

        rastaLockedCamera.SetActive(true);
    }

    public void UnlockFromRasta(Transform rastaTarget)
    {
        if (currentRastaTarget != rastaTarget)
        {
            return;
        }

        currentRastaTarget = null;
        isLockedOnRasta = false;

        if (rastaLockedCamera != null)
        {
            rastaLockedCamera.SetActive(false);
        }

        if (previousActiveCamera != null)
        {
            previousActiveCamera.SetActive(true);
        }
        else if (normalCameras.Length > 0 && normalCameras[0] != null)
        {
            normalCameras[0].SetActive(true);
        }
    }

    private void RotatePlayerTowardRasta()
    {
        if (playerTransform == null || currentRastaTarget == null)
        {
            return;
        }

        Vector3 direction = currentRastaTarget.position - playerTransform.position;
        direction.y = 0f;

        if (direction.sqrMagnitude <= 0.001f)
        {
            return;
        }

        Quaternion targetRotation = Quaternion.LookRotation(direction);

        playerTransform.rotation = Quaternion.Slerp(
            playerTransform.rotation,
            targetRotation,
            playerRotationSpeed * Time.deltaTime
        );
    }

    private void RotateCameraPivotTowardRasta()
    {
        if (cameraPivot == null || currentRastaTarget == null)
        {
            return;
        }

        Vector3 targetPosition = currentRastaTarget.position + rastaLookOffset;
        Vector3 direction = targetPosition - cameraPivot.position;

        Vector3 localDirection = playerTransform.InverseTransformDirection(direction.normalized);

        float targetPitch = -Mathf.Atan2(localDirection.y, localDirection.z) * Mathf.Rad2Deg;
        targetPitch = Mathf.Clamp(targetPitch, minCameraPitch, maxCameraPitch);

        Quaternion targetRotation = Quaternion.Euler(targetPitch, 0f, 0f);

        cameraPivot.localRotation = Quaternion.Slerp(
            cameraPivot.localRotation,
            targetRotation,
            cameraPitchSpeed * Time.deltaTime
        );
    }

    private GameObject GetCurrentActiveNormalCamera()
    {
        foreach (GameObject normalCamera in normalCameras)
        {
            if (normalCamera != null && normalCamera.activeSelf)
            {
                return normalCamera;
            }
        }

        return null;
    }
}