using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEffects : MonoBehaviour
{
    public enum EffectType
    {
        SpeedMultiplier,
        DamageMultiplier,
        Cannabis
    }

    public IReadOnlyDictionary<string, EffectData> ActiveEffectData => activeEffectData;

    private PlayerController playerController;
    private CameraEffects cameraEffects;

    private readonly Dictionary<string, Coroutine> activeEffects = new();
    private readonly Dictionary<string, EffectData> activeEffectData = new();

    private void Awake()
    {
        playerController = GetComponent<PlayerController>();
        cameraEffects = FindAnyObjectByType<CameraEffects>();
    }

    public void ApplyTimedEffect(string effectId, EffectType effectType, float value, float duration)
    {
        if (activeEffects.TryGetValue(effectId, out Coroutine runningEffect))
        {
            StopCoroutine(runningEffect);

            if (activeEffectData.TryGetValue(effectId, out EffectData existingData))
            {
                RemoveEffect(existingData);
            }
        }

        EffectData newEffectData = new EffectData(effectId, effectType, value, duration);
        Coroutine newCoroutine = StartCoroutine(EffectRoutine(newEffectData));

        activeEffects[effectId] = newCoroutine;
        activeEffectData[effectId] = newEffectData;
    }

    private IEnumerator EffectRoutine(EffectData effectData)
    {
        ApplyEffect(effectData);

        yield return new WaitForSeconds(effectData.Duration);

        RemoveEffect(effectData);
        activeEffects.Remove(effectData.Id);
        activeEffectData.Remove(effectData.Id);
    }

    private void ApplyEffect(EffectData effectData)
    {
        switch (effectData.Type)
        {
            case EffectType.SpeedMultiplier:
                playerController.speed *= effectData.Value;
                break;

            case EffectType.DamageMultiplier:
                playerController.damageMultiplier *= effectData.Value;
                break;

            case EffectType.Cannabis:
                if (cameraEffects != null)
                {
                    cameraEffects.PlayCannabisEffect(effectData.Duration);
                }
                break;
        }
    }

    private void RemoveEffect(EffectData effectData)
    {
        switch (effectData.Type)
        {
            case EffectType.SpeedMultiplier:
                playerController.speed /= effectData.Value;
                break;

            case EffectType.DamageMultiplier:
                playerController.damageMultiplier /= effectData.Value;
                break;

            case EffectType.Cannabis:
                // Rien à faire ici : CameraEffects remet déjà l'écran normal à la fin.
                break;
        }
    }

    public bool HasActiveEffect(string effectId)
    {
        return activeEffectData.ContainsKey(effectId);
    }

    public class EffectData
    {
        public string Id { get; }
        public EffectType Type { get; }
        public float Value { get; }
        public float Duration { get; }
        public float EndTime { get; }

        public float RemainingTime => Mathf.Max(0f, EndTime - Time.time);

        public EffectData(string id, EffectType type, float value, float duration)
        {
            Id = id;
            Type = type;
            Value = value;
            Duration = duration;
            EndTime = Time.time + duration;
        }
    }
}