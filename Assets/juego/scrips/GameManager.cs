using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManager : MonoBehaviour
{
    public enum EstadoJuego
    {
        Inicio,Jugando,Pausa,Victoria,Derrota
    }

    [Header("Estado del juego")]
    [SerializeField] private EstadoJuego estadoActual = EstadoJuego.Inicio;

    [Header("Vida y puntaje")]
    [SerializeField] private int vidaMaxima = 10;
    [SerializeField] private int vidaActual = 10;
    [SerializeField] private int puntaje = 0;

    [Header("Textos de la interfaz")]
    [SerializeField] private TMP_Text textoVida;
    [SerializeField] private TMP_Text textoPuntaje;

    [Header("Paneles de la interfaz")]
    [SerializeField] private GameObject panelInicio;
    [SerializeField] private GameObject panelPausa;
    [SerializeField] private GameObject panelVictoria;
    [SerializeField] private GameObject panelDerrota;

    [Header("Puerta de salida")]
    [SerializeField] private Transform puerta;
    [SerializeField] private float distanciaMovimientoPuerta = 0.1f;

    private Vector3 posicionInicialPuerta;

    public EstadoJuego EstadoActual => estadoActual;
    public int VidaActual => vidaActual;
    public int Puntaje => puntaje;

    private void Start()
    {
        Time.timeScale = 1f;
        vidaActual = vidaMaxima;
        puntaje = 0;
        if (puerta != null)
        {
            posicionInicialPuerta = puerta.position;
        }
        ActualizarInterfaz();
        CambiarEstado(EstadoJuego.Inicio);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            if (estadoActual == EstadoJuego.Jugando)
            {
                PausarJuego();
            }
            else if (estadoActual == EstadoJuego.Pausa)
            {
                ReanudarJuego();
            }
        }
        if (estadoActual == EstadoJuego.Inicio &&
            Input.GetKeyDown(KeyCode.Return))
        {
            ComenzarJuego();
        }
        if (estadoActual == EstadoJuego.Jugando &&
            puerta != null)
        {
            float distancia = Vector3.Distance(posicionInicialPuerta,puerta.position);
            if (distancia > distanciaMovimientoPuerta)
            {
                GanarJuego();
            }
        }
    }

    public void ComenzarJuego()
    {
        if (estadoActual != EstadoJuego.Inicio) return;
        Time.timeScale = 1f;
        CambiarEstado(EstadoJuego.Jugando);
    }

    public void PausarJuego()
    {
        if (estadoActual != EstadoJuego.Jugando) return;
        Time.timeScale = 0f;
        CambiarEstado(EstadoJuego.Pausa);
    }

    public void ReanudarJuego()
    {
        if (estadoActual != EstadoJuego.Pausa) return;
        Time.timeScale = 1f;
        CambiarEstado(EstadoJuego.Jugando);
    }

    public void SumarPuntos(int cantidad)
    {
        if (estadoActual != EstadoJuego.Jugando) return;
        puntaje += Mathf.Max(0, cantidad);
        ActualizarInterfaz();
        Debug.Log("Puntaje actual: " + puntaje);
    }

    public void RecibirDano(int cantidad)
    {
        if (estadoActual != EstadoJuego.Jugando) return;
        vidaActual -= Mathf.Max(0, cantidad);
        vidaActual = Mathf.Clamp(vidaActual, 0, vidaMaxima);
        ActualizarInterfaz();
        Debug.Log("Vida actual: " + vidaActual);
        if (vidaActual <= 0)
        {
            PerderJuego();
        }
    }
    public void Curar(int cantidad)
    {
        if (estadoActual != EstadoJuego.Jugando) return;

        vidaActual += Mathf.Max(0, cantidad);
        vidaActual = Mathf.Clamp(vidaActual, 0, vidaMaxima);
        ActualizarInterfaz();
        Debug.Log("Vida actual: " + vidaActual);
    }

    public void GanarJuego()
    {
        if (estadoActual != EstadoJuego.Jugando) return;
        Time.timeScale = 1f;
        CambiarEstado(EstadoJuego.Victoria);
    }

    public void PerderJuego()
    {
        if (estadoActual != EstadoJuego.Jugando) return;
        Time.timeScale = 1f;
        CambiarEstado(EstadoJuego.Derrota);
    }

    public void Reintentar()
    {
        Time.timeScale = 1f; 
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    private void ActualizarInterfaz()
    {
        if (textoVida != null)
        {
            textoVida.text = "Vida: " + vidaActual + " / " + vidaMaxima;
        }

        if (textoPuntaje != null)
        {
            textoPuntaje.text = "Puntaje: " + puntaje;
        }
    }

    private void CambiarEstado(EstadoJuego nuevoEstado)
    {
        estadoActual = nuevoEstado;

        if (panelInicio != null) panelInicio.SetActive(estadoActual == EstadoJuego.Inicio);

        if (panelPausa != null) panelPausa.SetActive(estadoActual == EstadoJuego.Pausa);

        if (panelVictoria != null)panelVictoria.SetActive(estadoActual == EstadoJuego.Victoria);

        if (panelDerrota != null)panelDerrota.SetActive(estadoActual == EstadoJuego.Derrota);
        Debug.Log("Estado del juego: " + estadoActual);
        Debug.Log("Vida: " + vidaActual + " | Puntaje: " + puntaje);
    }

    private void OnDestroy()
    {
        Time.timeScale = 1f;
    }
}