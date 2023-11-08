using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

public class UIPauseMenu : MonoBehaviour
{
    public UIDocument pauseMenu, gameMenu;
    private VisualElement pauseRoot, pauseContainer, gameRoot, gameContainer;
    private Button resumeButton, mainMenuButton, exitButton;
    private PlayerInput input;
    private InputActionMap actionMap;
    public bool paused;

    void Start()
    {
        input = SelectionManager.instance.gameObject.GetComponent<PlayerInput>();

        gameRoot = gameMenu.rootVisualElement;
        pauseRoot = pauseMenu.rootVisualElement;

        gameContainer = gameRoot.Q<VisualElement>("Container");
        pauseContainer = pauseRoot.Q<VisualElement>("Container");

        paused = false;
        ResumeGame();
    }

    public void ExitGame()
    {
        AnalyticsManager.s.ReportData();
        Application.Quit();
    }

    public void ReturnMainMenu()
    {
        AnalyticsManager.s.ReportData();
        Time.timeScale = 1;
        SceneManager.UnloadSceneAsync("User Interface");
        SceneManager.UnloadSceneAsync(SceneManager.GetActiveScene());
        SceneManager.LoadScene("Main Menu");
    }

    public void TogglePause()
    {
        paused = !paused;
        if (paused)
        {
            PauseGame();
        }
        else
        {
            ResumeGame();
        }

    }

    public void PauseGame()
    {
        input.actions.Disable();
        SelectionManager.instance.gameObject.GetComponent<PlayerInput>().SwitchCurrentActionMap("UI");
        input.actions.Enable();
        Time.timeScale = 0;
        pauseContainer.visible = true;
        pauseContainer.style.display = DisplayStyle.Flex;
        gameContainer.style.display = DisplayStyle.None;
        SelectionManager.instance.gameObject.GetComponent<Camera>().enabled = false;

    }

    public void ResumeGame()
    {
        input.actions.Disable();
        SelectionManager.instance.gameObject.GetComponent<PlayerInput>().SwitchCurrentActionMap("Player");
        input.actions.Enable();
        Time.timeScale = 1;
        pauseContainer.visible = false;
        pauseContainer.style.display = DisplayStyle.None;
        gameContainer.style.display = DisplayStyle.Flex;
        SelectionManager.instance.gameObject.GetComponent<Camera>().enabled = true;
    }
}
