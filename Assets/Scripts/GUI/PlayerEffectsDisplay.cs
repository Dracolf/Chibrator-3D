using System.Text;
using TMPro;
using UnityEngine;

public class PlayerEffectsDisplay : MonoBehaviour
{
    [SerializeField]
    private PlayerEffects playerEffects;

    [SerializeField]
    private TextMeshProUGUI effectsText;

    private readonly StringBuilder stringBuilder = new();

    private void Awake()
    {
        if (playerEffects == null)
        {
            playerEffects = FindAnyObjectByType<PlayerEffects>();
        }
    }

    private void Update()
    {
        if (playerEffects == null || effectsText == null)
        {
            return;
        }

        if (playerEffects.ActiveEffectData.Count == 0)
        {
            effectsText.text = "";
            return;
        }

        stringBuilder.Clear();

        foreach (PlayerEffects.EffectData effectData in playerEffects.ActiveEffectData.Values)
        {
            string effectName = GetEffectDisplayName(effectData.Type);
            string effectIcon = GetEffectIcon(effectData.Type);

            stringBuilder.Append(effectIcon);
            stringBuilder.Append(" ");
            stringBuilder.Append(effectName);
            stringBuilder.Append(" : ");
            stringBuilder.Append(effectData.RemainingTime.ToString("0.0"));
            stringBuilder.Append("s");
            stringBuilder.AppendLine();
        }

        effectsText.text = stringBuilder.ToString();
    }

    private string GetEffectDisplayName(PlayerEffects.EffectType effectType)
    {
        switch (effectType)
        {
            case PlayerEffects.EffectType.SpeedMultiplier:
                return "Vitesse x2";

            case PlayerEffects.EffectType.DamageMultiplier:
                return "Dégâts x2";

            default:
                return "Effet";
        }
    }

    private string GetEffectIcon(PlayerEffects.EffectType effectType)
    {
        switch (effectType)
        {
            case PlayerEffects.EffectType.SpeedMultiplier:
                return "⚡";

            case PlayerEffects.EffectType.DamageMultiplier:
                return "💥";

            default:
                return "●";
        }
    }
}