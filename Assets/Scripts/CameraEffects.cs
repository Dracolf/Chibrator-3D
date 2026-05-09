using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class CameraEffects : MonoBehaviour
{
    [Header("Shake")]
    [SerializeField]
    private Transform shakeTarget;

    [SerializeField]
    private float defaultShakeDuration = 0.25f;

    [SerializeField]
    private float defaultShakeStrength = 0.25f;

    [Header("Damage Flash")]
    [SerializeField]
    private Image damageOverlay;

    [SerializeField]
    private float damageFlashAlpha = 0.45f;

    [SerializeField]
    private float damageFlashFadeDuration = 0.35f;

    private Vector3 originalLocalPosition;
    private Coroutine shakeCoroutine;
    private Coroutine damageFlashCoroutine;

    private void Awake()
    {
        if (shakeTarget == null)
        {
            shakeTarget = transform;
        }

        originalLocalPosition = shakeTarget.localPosition;

        if (damageOverlay != null)
        {
            SetDamageOverlayAlpha(0f);
        }
    }

    public void PlayShake()
    {
        PlayShake(defaultShakeDuration, defaultShakeStrength);
    }

    public void PlayShake(float duration, float strength)
    {
        if (shakeTarget == null)
        {
            return;
        }

        if (shakeCoroutine != null)
        {
            StopCoroutine(shakeCoroutine);
            shakeTarget.localPosition = originalLocalPosition;
        }

        shakeCoroutine = StartCoroutine(ShakeRoutine(duration, strength));
    }

    public void PlayDamageFlash()
    {
        if (damageOverlay == null)
        {
            return;
        }

        if (damageFlashCoroutine != null)
        {
            StopCoroutine(damageFlashCoroutine);
        }

        damageFlashCoroutine = StartCoroutine(DamageFlashRoutine());
    }

    private IEnumerator ShakeRoutine(float duration, float strength)
    {
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            Vector3 randomOffset = Random.insideUnitSphere * strength;
            randomOffset.z = 0f;

            shakeTarget.localPosition = originalLocalPosition + randomOffset;

            elapsedTime += Time.unscaledDeltaTime;
            yield return null;
        }

        shakeTarget.localPosition = originalLocalPosition;
        shakeCoroutine = null;
    }

    private IEnumerator DamageFlashRoutine()
    {
        float elapsedTime = 0f;

        SetDamageOverlayAlpha(damageFlashAlpha);

        while (elapsedTime < damageFlashFadeDuration)
        {
            elapsedTime += Time.unscaledDeltaTime;

            float alpha = Mathf.Lerp(
                damageFlashAlpha,
                0f,
                elapsedTime / damageFlashFadeDuration
            );

            SetDamageOverlayAlpha(alpha);

            yield return null;
        }

        SetDamageOverlayAlpha(0f);
        damageFlashCoroutine = null;
    }

    private void SetDamageOverlayAlpha(float alpha)
    {
        Color color = damageOverlay.color;
        color.a = alpha;
        damageOverlay.color = color;
    }
}