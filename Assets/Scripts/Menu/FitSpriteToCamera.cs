using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class FitSpriteToCamera : MonoBehaviour
{
    [SerializeField]
    private Camera targetCamera;

    [SerializeField]
    private float distanceFromCamera = 10f;

    [SerializeField]
    private bool preserveAspectRatio = true;

    private SpriteRenderer spriteRenderer;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();

        if (targetCamera == null)
        {
            targetCamera = Camera.main;
        }
    }

    private void Start()
    {
        FitToCamera();
    }

    private void FitToCamera()
    {
        if (targetCamera == null || spriteRenderer.sprite == null)
        {
            return;
        }

        transform.position = targetCamera.transform.position + targetCamera.transform.forward * distanceFromCamera;
        transform.rotation = targetCamera.transform.rotation;

        float height = 2f * distanceFromCamera * Mathf.Tan(targetCamera.fieldOfView * 0.5f * Mathf.Deg2Rad);
        float width = height * targetCamera.aspect;

        Vector2 spriteSize = spriteRenderer.sprite.bounds.size;

        float scaleX = width / spriteSize.x;
        float scaleY = height / spriteSize.y;

        if (preserveAspectRatio)
        {
            float scale = Mathf.Max(scaleX, scaleY);
            transform.localScale = new Vector3(scale, scale, 1f);
        }
        else
        {
            transform.localScale = new Vector3(scaleX, scaleY, 1f);
        }
    }
}