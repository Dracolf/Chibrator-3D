using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI lastScore;
    [SerializeField]
    private TextMeshProUGUI highScore;

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
    }
    public void LoadScene(string sceneName)
    {
        SceneManager.LoadSceneAsync(sceneName);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
