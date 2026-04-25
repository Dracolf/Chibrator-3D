using UnityEngine;

public class Capote : MonoBehaviour
{
    [SerializeField]
    private float speed;

    [SerializeField]
    private Transform visual;

    public int damages = 5;

    private Vector3 moveDirection;

    public void Init(Vector3 startPosition, Vector3 direction)
    {
        transform.position = startPosition;
        moveDirection = direction.normalized;
    }

    void Update()
    {
        transform.position += moveDirection * speed * Time.deltaTime;
        visual.Rotate(0f, 300f * Time.deltaTime, 0f);
    }

    private void OnTriggerEnter(Collider other)
    {
        ZiziController zizi = other.GetComponent<ZiziController>();

        if (zizi != null)
        {
            zizi.TakeDamage(damages);
            Destroy(gameObject);
        }
    }
}