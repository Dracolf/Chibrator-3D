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

    [Header("Cannabis Effect")]
    [SerializeField]
    private Image cannabisOverlay;

    [SerializeField]
    private float cannabisOverlayAlpha = 0.25f;

    [SerializeField]
    private float cannabisFadeDuration = 0.75f;

    [SerializeField]
    private float cannabisWobblePositionStrength = 0.08f;

    [SerializeField]
    private float cannabisWobbleRotationStrength = 2.5f;

    [SerializeField]
    private float cannabisWobbleSpeed = 3f;

    [SerializeField]
    private AudioSource audioSource;

    [SerializeField]
    private AudioClip baseMusic, fayaGanjah;

    private Vector3 originalLocalPosition;
    private Quaternion originalLocalRotation;

    private Coroutine shakeCoroutine;
    private Coroutine damageFlashCoroutine;
    private Coroutine cannabisCoroutine;

    private void Awake()
    {
        if (shakeTarget == null)
        {
            shakeTarget = transform;
        }

        originalLocalPosition = shakeTarget.localPosition;
        originalLocalRotation = shakeTarget.localRotation;

        if (damageOverlay != null)
        {
            SetImageAlpha(damageOverlay, 0f);
        }

        if (cannabisOverlay != null)
        {
            SetImageAlpha(cannabisOverlay, 0f);
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

    public void PlayCannabisEffect(float duration = 25f)
    {
        if (shakeTarget == null)
        {
            return;
        }

        if (cannabisCoroutine != null)
        {
            StopCoroutine(cannabisCoroutine);
            ResetCannabisEffect();
        }

        cannabisCoroutine = StartCoroutine(CannabisRoutine(duration));
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

        SetImageAlpha(damageOverlay, damageFlashAlpha);

        while (elapsedTime < damageFlashFadeDuration)
        {
            elapsedTime += Time.unscaledDeltaTime;

            float alpha = Mathf.Lerp(
                damageFlashAlpha,
                0f,
                elapsedTime / damageFlashFadeDuration
            );

            SetImageAlpha(damageOverlay, alpha);

            yield return null;
        }

        SetImageAlpha(damageOverlay, 0f);
        damageFlashCoroutine = null;
    }

    private IEnumerator CannabisRoutine(float duration)
    {
        audioSource.clip = fayaGanjah;
        audioSource.Play();

        float elapsedTime = 0f;

        while (elapsedTime < cannabisFadeDuration)
        {
            elapsedTime += Time.deltaTime;

            float alpha = Mathf.Lerp(
                0f,
                cannabisOverlayAlpha,
                elapsedTime / cannabisFadeDuration
            );

            SetImageAlpha(cannabisOverlay, alpha);

            yield return null;
        }

        elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;

            float wave = Mathf.Sin(Time.time * cannabisWobbleSpeed);
            float secondWave = Mathf.Sin(Time.time * cannabisWobbleSpeed * 1.7f);

            Vector3 wobbleOffset = new Vector3(
                wave * cannabisWobblePositionStrength,
                secondWave * cannabisWobblePositionStrength,
                0f
            );

            Quaternion wobbleRotation = Quaternion.Euler(
                secondWave * cannabisWobbleRotationStrength,
                wave * cannabisWobbleRotationStrength,
                wave * cannabisWobbleRotationStrength
            );

            shakeTarget.localPosition = originalLocalPosition + wobbleOffset;
            shakeTarget.localRotation = originalLocalRotation * wobbleRotation;

            yield return null;
        }

        elapsedTime = 0f;

        Vector3 startPosition = shakeTarget.localPosition;
        Quaternion startRotation = shakeTarget.localRotation;

        while (elapsedTime < cannabisFadeDuration)
        {
            elapsedTime += Time.deltaTime;

            float t = elapsedTime / cannabisFadeDuration;

            shakeTarget.localPosition = Vector3.Lerp(startPosition, originalLocalPosition, t);
            shakeTarget.localRotation = Quaternion.Slerp(startRotation, originalLocalRotation, t);

            float alpha = Mathf.Lerp(cannabisOverlayAlpha, 0f, t);
            SetImageAlpha(cannabisOverlay, alpha);

            yield return null;
        }

        ResetCannabisEffect();
        cannabisCoroutine = null;
    }

    private void ResetCannabisEffect()
    {
        audioSource.clip = baseMusic;
        audioSource.Play();
        
        if (shakeTarget != null)
        {
            shakeTarget.localPosition = originalLocalPosition;
            shakeTarget.localRotation = originalLocalRotation;
        }

        if (cannabisOverlay != null)
        {
            SetImageAlpha(cannabisOverlay, 0f);
        }
    }

    private void SetImageAlpha(Image image, float alpha)
    {
        if (image == null)
        {
            return;
        }

        Color color = image.color;
        color.a = alpha;
        image.color = color;
    }
}