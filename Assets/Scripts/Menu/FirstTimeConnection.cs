using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class FirstTimeConnection : MonoBehaviour
{
    [SerializeField]
    private GameObject mainCanvas, firstTimeScreen, chibrator;
    [SerializeField]
    private TMP_InputField inputField;
    [SerializeField]
    private Button validate;
    [SerializeField]
    private TextMeshProUGUI error;
    private SoundEffectPlayer soundEffectPlayer;
    private ChibratorApiClient apiClient;

    private void Awake()
    {
        if (PlayerPrefs.GetInt("hasAlreadyPlay") != 1)
        {
            mainCanvas.SetActive(false);
            chibrator.SetActive(false);
            firstTimeScreen.SetActive(true);
        } else
        {
            mainCanvas.SetActive(true);
            chibrator.SetActive(true);
            firstTimeScreen.SetActive(false);
        }

        soundEffectPlayer = FindAnyObjectByType<SoundEffectPlayer>();
        apiClient = FindAnyObjectByType<ChibratorApiClient>();
    }

    public void SavePseudo()
    {
        if (string.IsNullOrWhiteSpace(inputField.text))
        {
            error.gameObject.SetActive(true);
            soundEffectPlayer.PlaySound(SoundEffectType.Error);
        } else
        {
            apiClient.GetPlayerStats(inputField.text, (success, response) =>
            {
                if (success)
                {
                    error.gameObject.SetActive(true);
                    error.text = "Ce pseudo est déjà pris.";
                    soundEffectPlayer.PlaySound(SoundEffectType.Error);
                }
                else
                {
                    PlayerPrefs.SetString("Pseudo", inputField.text);
                    PlayerPrefs.SetInt("hasAlreadyPlay", 1);
                    PlayerPrefs.SetInt("HighScore", 0);
                    PlayerPrefs.SetInt("LastScore", 0);
                    mainCanvas.SetActive(true);
                    chibrator.SetActive(true);
                    firstTimeScreen.SetActive(false);
                    soundEffectPlayer.PlaySound(SoundEffectType.SoftProut);
                    PlayerPrefs.Save();
                }
            });
        }
    }
}
