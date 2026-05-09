using System.Collections;
using TMPro;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class MainMenuManager : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI lastScore;

    [SerializeField]
    private TextMeshProUGUI highScore;

    [SerializeField]
    private Button start, quit, shop, credits;

    [SerializeField]
    private Button shopBackButton, creditsBackButton;

    [SerializeField]
    private TextMeshProUGUI copyright, version, notEnough;

    [SerializeField]
    private Image logo, creditsSheet, shopMenu;

    [SerializeField]
    private GameObject chibrator;

    [SerializeField]
    private SoundEffectPlayer soundEffectPlayer;

    private void Awake()
    {
        if (!PlayerPrefs.HasKey("LastScore"))
        {
            PlayerPrefs.SetInt("LastScore", 0);
        }

        if (!PlayerPrefs.HasKey("HighScore"))
        {
            PlayerPrefs.SetInt("HighScore", 0);
        }

        PlayerPrefs.Save();

        lastScore.text = "Last score : " + PlayerPrefs.GetInt("LastScore");
        highScore.text = "Highscore : " + PlayerPrefs.GetInt("HighScore");

        BackToMainMenu();

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    private IEnumerator Start()
    {
        yield return null;
        SelectButton(start);
    }

    private void SelectButton(Button button)
    {
        if (button == null || EventSystem.current == null)
        {
            return;
        }

        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(button.gameObject);
    }

    public void LoadScene(string sceneName)
    {
        soundEffectPlayer.PlaySound(SoundEffectType.SoftProut);
        SceneManager.LoadSceneAsync(sceneName);
    }

    public void Credits()
    {
        lastScore.gameObject.SetActive(false);
        highScore.gameObject.SetActive(false);
        copyright.gameObject.SetActive(false);
        version.gameObject.SetActive(false);
        chibrator.SetActive(false);
        logo.gameObject.SetActive(false);
        start.gameObject.SetActive(false);
        quit.gameObject.SetActive(false);
        shop.gameObject.SetActive(false);
        credits.gameObject.SetActive(false);
        creditsSheet.gameObject.SetActive(true);
        shopMenu.gameObject.SetActive(false);

        SelectButton(creditsBackButton);
        soundEffectPlayer.PlaySound(SoundEffectType.SoftProut);
    }

    public void Shop()
    {
        lastScore.gameObject.SetActive(false);
        highScore.gameObject.SetActive(false);
        copyright.gameObject.SetActive(false);
        version.gameObject.SetActive(false);
        chibrator.SetActive(false);
        logo.gameObject.SetActive(false);
        start.gameObject.SetActive(false);
        quit.gameObject.SetActive(false);
        shop.gameObject.SetActive(false);
        credits.gameObject.SetActive(false);
        creditsSheet.gameObject.SetActive(false);
        shopMenu.gameObject.SetActive(true);

        SelectButton(shopBackButton);
        soundEffectPlayer.PlaySound(SoundEffectType.SoftProut);
    }

    public void BackToMainMenu()
    {
        lastScore.gameObject.SetActive(true);
        highScore.gameObject.SetActive(true);
        copyright.gameObject.SetActive(true);
        version.gameObject.SetActive(true);
        chibrator.SetActive(true);
        logo.gameObject.SetActive(true);
        start.gameObject.SetActive(true);
        quit.gameObject.SetActive(true);
        shop.gameObject.SetActive(true);
        credits.gameObject.SetActive(true);
        creditsSheet.gameObject.SetActive(false);
        shopMenu.gameObject.SetActive(false);
        notEnough.gameObject.SetActive(false);

        SelectButton(start);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}