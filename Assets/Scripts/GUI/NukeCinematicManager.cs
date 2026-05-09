using System.Linq;
using UnityEngine;
using UnityEngine.Video;

public class NukeCinematicManager : MonoBehaviour
{
    [SerializeField]
    private GameObject cinematicPanel;

    [SerializeField]
    private VideoPlayer videoPlayer;

    [SerializeField]
    private GameObject missiles;

    private bool isPlaying;

    private void Awake()
    {
        if (cinematicPanel != null)
        {
            cinematicPanel.SetActive(false);
        }

        if (videoPlayer == null)
        {
            videoPlayer = GetComponent<VideoPlayer>();
        }

        if (videoPlayer != null)
        {
            videoPlayer.loopPointReached += OnCinematicFinished;
        }
    }

    public void PlayNukeCinematic()
    {
        if (isPlaying)
        {
            return;
        }

        isPlaying = true;

        Time.timeScale = 0f;

        if (cinematicPanel != null)
        {
            cinematicPanel.SetActive(true);
        }

        if (videoPlayer != null)
        {
            videoPlayer.time = 0;
            videoPlayer.Play();
        }
    }

    private void OnCinematicFinished(VideoPlayer source)
    {
        EndCinematic();
    }

    private void EndCinematic()
    {
        if (videoPlayer != null)
        {
            videoPlayer.Stop();
        }

        if (cinematicPanel != null)
        {
            cinematicPanel.SetActive(false);
        }

        Time.timeScale = 1f;
        isPlaying = false;

        if (missiles != null)
        {
            Instantiate(missiles, new Vector3(0f, 100f, 0f), Quaternion.identity);
        }
    }

    private void OnDestroy()
    {
        if (videoPlayer != null)
        {
            videoPlayer.loopPointReached -= OnCinematicFinished;
        }
    }
}