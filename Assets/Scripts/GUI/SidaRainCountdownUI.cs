using TMPro;
using UnityEngine;

public class SidaRainCountdownUI : MonoBehaviour
{
    [Header("UI")]
    [SerializeField]
    private GameObject sidaIcon;

    [SerializeField]
    private TextMeshProUGUI countdownText;

    public void ShowCountdown(float remainingTime)
    {
        if (sidaIcon != null)
        {
            sidaIcon.SetActive(true);
        }

        if (countdownText != null)
        {
            countdownText.gameObject.SetActive(true);
            countdownText.text = Mathf.CeilToInt(remainingTime).ToString() + "s";
        }
    }

    public void HideCountdown()
    {
        if (sidaIcon != null)
        {
            sidaIcon.SetActive(false);
        }

        if (countdownText != null)
        {
            countdownText.gameObject.SetActive(false);
        }
    }
}