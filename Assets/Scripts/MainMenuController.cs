using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{
    [Header("Nombre de la escena del juego")]
    public string gameSceneName = "GameScene";

    [Header("Paneles UI")]
    public GameObject optionsPanel;

    void Start()
    {
        Time.timeScale = 1;
        
        if (optionsPanel != null)
        {
            optionsPanel.SetActive(false);
        }
    }

    public void OnPlayButton()
    {
        SceneManager.LoadScene(gameSceneName);
    }

    public void OnOptionsButton()
    {
        if (optionsPanel != null)
        {
            optionsPanel.SetActive(true);
        }
    }

    public void OnCloseOptionsButton()
    {
        if (optionsPanel != null)
        {
            optionsPanel.SetActive(false);
        }
    }

    public void OnQuitButton()
    {
        Application.Quit();

        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #endif
    }
}