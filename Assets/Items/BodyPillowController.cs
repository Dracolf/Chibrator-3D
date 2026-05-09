using UnityEngine;

public class BodyPillowController : MonoBehaviour
{
    public float health = 2000f;

    private ZiziInfosDisplay infos;

    private void Start()
    {
        infos = GetComponentInChildren<ZiziInfosDisplay>();

        if (infos != null)
        {
            infos.SetMaxHealth(health);
            infos.SetHealth(health);
        }
    }

    public void TakeDamage(float amount)
    {
        health -= amount;

        if (infos != null)
        {
            infos.SetHealth(health);
        }

        if (health <= 0)
        {
            Destroy(gameObject);
        }
    }
}