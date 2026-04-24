using UnityEngine;

public class ZiziController : MonoBehaviour
{
    [SerializeField]
    private Transform target;

    [SerializeField]
    private int _avgSpeed = 3;

    public int speed;

    private void Start()
    {
        speed = Random.Range(_avgSpeed-1, _avgSpeed+2);
    }

    private void Update()
    {
        if (target == null)
            return;

        Vector3 direction = (target.position - transform.position).normalized;
        transform.position += direction * speed * Time.deltaTime;
    }

    private void OnCollisionStay(Collision collision)
{
    PlayerController player = collision.gameObject.GetComponent<PlayerController>();

    if (player != null)
    {
        player.ChangeHealth(-0.1f);
    }
}
}