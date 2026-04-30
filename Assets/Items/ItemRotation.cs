using UnityEngine;

public class ItemRotation : MonoBehaviour
{
    [SerializeField]
    private Transform visual;

    [SerializeField]
    private float rotationSpeed = 100f;

    private void Update()
    {
        visual.Rotate(0f, rotationSpeed * Time.deltaTime, 0f);
    }
}
