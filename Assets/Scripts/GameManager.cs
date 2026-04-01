using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("Configuración Recolección")]
    public int totalObjetosNecesarios = 3;
    private int objetosRecogidos = 0;

    [Header("UI")]
    public TextMeshProUGUI contadorTexto;
    public TextMeshProUGUI tiempoTexto;

    [Header("Navegación Niveles")]
    public string siguienteNivelNombre = "Nivel2";

    [Header("Sonidos")]
    public AudioSource audioSource;
    public AudioClip sonidoInicioNivel;
    public AudioClip sonidoRecolectar;
    
    private float tiempoActual = 0f;
    private bool nivelCompletado = false;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        ActualizarHUD();

        if (audioSource != null && sonidoInicioNivel != null)
        {
            audioSource.PlayOneShot(sonidoInicioNivel);
        }
    }
    
    void Update()
    {
        if (!nivelCompletado)
        {
            tiempoActual += Time.deltaTime;
            ActualizarTiempoUI();
        }
    }

    public void RecogerObjeto()
    {
        if (nivelCompletado) return;
        
        objetosRecogidos++;
        ActualizarHUD();

        if (audioSource != null && sonidoRecolectar != null)
        {
            audioSource.PlayOneShot(sonidoRecolectar);
        }

        if (objetosRecogidos >= totalObjetosNecesarios)
        {
            nivelCompletado = true;
            StartCoroutine(CompletarNivelConRetraso(4f));
        }
    }

    void ActualizarHUD()
    {
        if (contadorTexto != null)
        {
            contadorTexto.text = "Objetos: " + objetosRecogidos + " / " + totalObjetosNecesarios;
        }
    }
    
    void ActualizarTiempoUI()
    {
        if (tiempoTexto != null)
        {
            int minutos = Mathf.FloorToInt(tiempoActual / 60);
            int segundos = Mathf.FloorToInt(tiempoActual % 60);
            tiempoTexto.text = string.Format("Tiempo: {0:00}:{1:00}", minutos, segundos);
        }
    }

    System.Collections.IEnumerator CompletarNivelConRetraso(float tiempoEspera)
    {
        yield return new WaitForSeconds(tiempoEspera);
        SceneManager.LoadScene(siguienteNivelNombre);
    }
}
