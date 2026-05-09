using UnityEngine;

public class MissilesController : MonoBehaviour
{
    [SerializeField]
    private int speed = 15;
    private CameraEffects cameraEffects;

    private void Awake()
    {
        cameraEffects = FindAnyObjectByType<CameraEffects>();
    }

    private void Update()
    {
        transform.position += Vector3.down * speed * Time.deltaTime;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Ground"))
        {
            ZiziController[] zizis = FindObjectsByType<ZiziController>(FindObjectsSortMode.None);
            BossController[] bosses = FindObjectsByType<BossController>(FindObjectsSortMode.None);
            TerroristeController[] terroristes = FindObjectsByType<TerroristeController>(FindObjectsSortMode.None);

            foreach (ZiziController zizi in zizis)
            {
                zizi.TakeDamage(100f);
            }

            foreach (BossController boss in bosses)
            {
                boss.TakeDamage(100f);
            }

            foreach (TerroristeController terroriste in terroristes)
            {
                terroriste.TakeDamage(100f);
            }
            SoundEffectPlayer soundEffectPlayer = FindAnyObjectByType<SoundEffectPlayer>();
            soundEffectPlayer.PlaySound(SoundEffectType.Boom);
            if (cameraEffects != null)
            {
                cameraEffects.PlayShake(0.6f, 0.6f);
            }
            Destroy(gameObject);
        }
    }
}