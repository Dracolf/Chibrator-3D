using UnityEngine;

public class BodyPillowBarrierBuilder : MonoBehaviour
{
    [SerializeField]
    private GameObject pillowPrefab;

    [SerializeField]
    private int pillowCount = 8;

    [SerializeField]
    private float radius = 2f;

    private void Start()
    {
        for (int i = 0; i < pillowCount; i++)
        {
            float angle = i * Mathf.PI * 2f / pillowCount;

            Vector3 direction = new Vector3(
                Mathf.Cos(angle),
                0f,
                Mathf.Sin(angle)
            );

            Vector3 position = transform.position + direction * radius;

            GameObject pillow = Instantiate(
                pillowPrefab,
                position,
                Quaternion.identity,
                transform
            );

            pillow.transform.rotation = Quaternion.LookRotation(direction);
        }
    }
}