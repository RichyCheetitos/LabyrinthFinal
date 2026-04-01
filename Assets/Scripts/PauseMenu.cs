using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    [Header("UI")]
    public GameObject pauseMenuUI; // Solo el Panel aquí

    private bool isPaused = false;
    private Button botonReanudar;
    private Button botonSalir;

    public static PauseMenu Instance { get; private set; }

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        // ⬅️ Buscar botones en Awake, antes de que el Panel se desactive
        botonReanudar = pauseMenuUI.transform.Find("BotonReanuda").GetComponent<Button>();
        botonSalir    = pauseMenuUI.transform.Find("BotonSalir").GetComponent<Button>();

        botonReanudar.onClick.AddListener(Resume);
        botonSalir.onClick.AddListener(QuitToMainMenu);
    }

    void Start()
    {
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
    }

    void Update()
    {
        if (SceneManager.GetActiveScene().name == "MainMenu") return;

        if (Keyboard.current.escapeKey.wasPressedThisFrame)
        {
            if (isPaused) Resume();
            else Pause();
        }
    }

    public void Resume()
    {
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        isPaused = false;
    }

    public void Pause()
    {
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        isPaused = true;
    }

    public void QuitToMainMenu()
    {
        Time.timeScale = 1f;
        isPaused = false;
        SceneManager.LoadScene("MainMenu");
    }
}