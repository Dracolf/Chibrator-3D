using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Settings : MonoBehaviour
{
    [SerializeField]
    private Sprite mute, unmute;
    [SerializeField]
    private Button muteMenuMusic, muteGameMusic;
    private bool isMenuMute, isGameMute;
    [SerializeField]
    private AudioSource audioSource;
    private string sceneName;

    void Start()
    {
        if (!PlayerPrefs.HasKey("isMenuMute"))
        {
            PlayerPrefs.SetInt("isMenuMute", 0);
        }
        if (!PlayerPrefs.HasKey("isGameMute"))
        {
            PlayerPrefs.SetInt("isGameMute", 0);
        }
        PlayerPrefs.Save();

        isMenuMute = PlayerPrefs.GetInt("isMenuMute") == 0 ? false : true;
        isGameMute = PlayerPrefs.GetInt("isGameMute") == 0 ? false : true;

        muteMenuMusic.image.sprite = isMenuMute ? unmute : mute;
        muteGameMusic.image.sprite = isGameMute ? unmute : mute;

        sceneName = SceneManager.GetActiveScene().name;
    }

    public void ToggleMenuMute()
    {
        if (!isMenuMute)
        {
            if (sceneName == "Menu")
            {
                audioSource.Stop();
            } 
            muteMenuMusic.image.sprite = unmute;
            isMenuMute = true;
            PlayerPrefs.SetInt("isMenuMute", 1);
            PlayerPrefs.Save();
        } else
        {
            if (sceneName == "Menu")
            {
                audioSource.Play();
            } 
            muteMenuMusic.image.sprite = mute;
            isMenuMute = false;
            PlayerPrefs.SetInt("isMenuMute", 0);
            PlayerPrefs.Save();
        }
    }

    public void ToggleGameMute()
    {
        if (!isGameMute)
        {
            if (sceneName == "MainScene")
            {
                audioSource.Stop();
            } 
            muteGameMusic.image.sprite = unmute;
            isGameMute = true;
            PlayerPrefs.SetInt("isGameMute", 1);
            PlayerPrefs.Save();
        } else
        {
            if (sceneName == "MainScene")
            {
                audioSource.Play();
            } 
            muteGameMusic.image.sprite = mute;
            isGameMute = false;
            PlayerPrefs.SetInt("isGameMute", 0);
            PlayerPrefs.Save();
        }
    }
}
