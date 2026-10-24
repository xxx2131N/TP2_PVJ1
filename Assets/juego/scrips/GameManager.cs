using UnityEngine;

public class GameManager : MonoBehaviour
{
public enum EstadoJuego
{Inicio,Jugando,Pausa,Victoria,Derrota}

[Header("Estado del juego")]
[SerializeField] private EstadoJuego estadoActual = EstadoJuego.Inicio;

[Header("Estadisticas del jugador")]
[SerializeField] public int vidaMaxima = 10;
[SerializeField] public int vidaActual = 10;
[SerializeField] public int puntaje = 0;

public EstadoJuego EstadoActual => estadoActual;
public int VidaActual => vidaActual;
public int Puntaje => puntaje;

private void Start()
{
    Time.timeScale = 1f;

    CambiarEstado(EstadoJuego.Inicio);
}

private void Update()
{
    // Presionar P para pausar o reanudar.
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

    // Presionar Enter para comenzar desde el inicio.
    if (estadoActual == EstadoJuego.Inicio &&
        Input.GetKeyDown(KeyCode.Return))
    {
        ComenzarJuego();
    }
}

public void ComenzarJuego()
{
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

    Debug.Log("Puntaje actual: " + puntaje);
}

public void RecibirDano(int cantidad)
{
    if (estadoActual != EstadoJuego.Jugando) return;

    vidaActual -= Mathf.Max(0, cantidad);
    vidaActual = Mathf.Clamp(vidaActual, 0, vidaMaxima);

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

private void CambiarEstado(EstadoJuego nuevoEstado)
{
    estadoActual = nuevoEstado;

    Debug.Log("Estado del juego: " + estadoActual);
    Debug.Log("Vida: " + vidaActual + " | Puntaje: " + puntaje);
}

private void OnDestroy()
{
    Time.timeScale = 1f;
}

}
