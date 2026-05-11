using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PauseManager : MonoBehaviour
{
    public static PauseManager Instance { get; private set; }

    [Header("UI")]
    [SerializeField]
    private GameObject pauseMenu;

    [SerializeField]
    private GameObject gui;

    [Header("Gamepad Navigation")]
    [SerializeField]
    private GameObject firstSelectedButton;

    public bool IsPaused { get; private set; }

    private readonly List<Gamepad> disabledGamepads = new List<Gamepad>();

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        pauseMenu.SetActive(false);
        gui.SetActive(true);

        Time.timeScale = 1f;
        AudioListener.pause = false;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        ClearSelectedButton();
    }

    public void TogglePause(bool openedWithGamepad)
    {
        if (IsPaused)
        {
            ResumeGame();
        }
        else
        {
            PauseGame(openedWithGamepad);
        }
    }

    // Surcharge pratique si jamais tu veux appeler TogglePause depuis un bouton ou un autre script.
    public void TogglePause()
    {
        TogglePause(false);
    }

    public void PauseGame(bool openedWithGamepad)
    {
        IsPaused = true;

        pauseMenu.SetActive(true);
        gui.SetActive(false);

        Time.timeScale = 0f;
        AudioListener.pause = true;

        Cursor.lockState = CursorLockMode.None;

        if (openedWithGamepad)
        {
            Cursor.visible = false;
            SelectFirstButton();
        }
        else
        {
            Cursor.visible = true;
            ClearSelectedButton();
            DisableGamepadsDuringPause();
        }
    }

    public void ResumeGame()
    {
        IsPaused = false;

        EnableGamepadsAfterPause();

        pauseMenu.SetActive(false);
        gui.SetActive(true);

        Time.timeScale = 1f;
        AudioListener.pause = false;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        ClearSelectedButton();
    }

    public void BackToMenu()
    {
        IsPaused = false;

        EnableGamepadsAfterPause();

        Time.timeScale = 1f;
        AudioListener.pause = false;

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        ClearSelectedButton();

        SceneManager.LoadScene("Menu");
    }

    private void SelectFirstButton()
    {
        if (EventSystem.current == null || firstSelectedButton == null)
        {
            return;
        }

        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(firstSelectedButton);
    }

    private void ClearSelectedButton()
    {
        if (EventSystem.current != null)
        {
            EventSystem.current.SetSelectedGameObject(null);
        }
    }

    private void DisableGamepadsDuringPause()
    {
        disabledGamepads.Clear();

        foreach (Gamepad gamepad in Gamepad.all)
        {
            if (gamepad == null || !gamepad.enabled)
            {
                continue;
            }

            disabledGamepads.Add(gamepad);
            InputSystem.DisableDevice(gamepad);
        }
    }

    private void EnableGamepadsAfterPause()
    {
        foreach (Gamepad gamepad in disabledGamepads)
        {
            if (gamepad == null)
            {
                continue;
            }

            InputSystem.EnableDevice(gamepad);
        }

        disabledGamepads.Clear();
    }
}