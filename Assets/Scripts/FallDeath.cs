using UnityEngine;
using UnityEngine.SceneManagement;

public class FallDeath : MonoBehaviour
{
    private void OnTriggerEnter(Collider player)
    {
        Score score = FindAnyObjectByType<Score>();

        PlayerPrefs.SetInt("LastScore", score.score);
        if (score.score > PlayerPrefs.GetInt("HighScore", 0))
        {
            PlayerPrefs.SetInt("HighScore", score.score);
        }
        PlayerPrefs.Save();

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        SceneManager.LoadScene("Menu");
    }
}
