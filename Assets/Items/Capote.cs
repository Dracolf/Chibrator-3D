using UnityEngine;

public class Capote : MonoBehaviour
{
    [SerializeField]
    private float speed;

    [SerializeField]
    private Transform visual;

    public float damages;

    private Vector3 moveDirection;

    public void Init(Vector3 startPosition, Vector3 direction, float damageAmount)
    {
        transform.position = startPosition;
        moveDirection = direction.normalized;
        damages = damageAmount;
    }

    void Update()
    {
        transform.position += moveDirection * speed * Time.deltaTime;
        visual.Rotate(0f, 300f * Time.deltaTime, 0f);
    }

    private void OnTriggerEnter(Collider other)
    {
        ZiziController zizi = other.GetComponent<ZiziController>();
        BossController boss = other.GetComponent<BossController>();
        TerroristeController terroriste = other.GetComponent<TerroristeController>();

        if (zizi != null)
        {
            zizi.TakeDamage(damages);
            Destroy(gameObject);
        }
        
        if (boss != null)
        {
            boss.TakeDamage(damages);
            Destroy(gameObject);
        }

        if (terroriste != null)
        {
            terroriste.TakeDamage(damages);
            Destroy(gameObject);
        }
    }
}