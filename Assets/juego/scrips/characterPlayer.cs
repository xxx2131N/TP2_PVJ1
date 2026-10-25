using UnityEngine;

public class characterPlayer : MonoBehaviour
{
    [Header("Vida del jugador")]
    [SerializeField] private int vidaJugador = 10;

    [Header("Ataque")]
    [SerializeField] private GameObject mano2;
    [SerializeField] private float dañoAtaque = 2f;
    [SerializeField] private float distanciaAtaque = 2f;
    [SerializeField] private float tiempoAtaque = 1f;
    [SerializeField] private float radioGolpe = 0.5f;
    [SerializeField] private LayerMask capaEnemigo;

    [Header("Puntaje")]
    [SerializeField] public int almacenarPuntaje = 0;
    private PowerUpPlayer powerUp;

    private GameManager gameManager;
    private bool jugadorMuerto = false;

    private Vector3 posicionInicialMano;
    private bool atacando = false;
    private bool golpeAplicado = false;
    private float tiempoActual = 0f;

    public int Puntaje => almacenarPuntaje;
    public int VidaJugador => gameManager != null? gameManager.VidaActual:vidaJugador;
    public bool JugadorMuerto => jugadorMuerto;
    public bool EstaVisible => powerUp == null || powerUp.EstaVisible;

    private void Start()
    {
        powerUp = GetComponent<PowerUpPlayer>();
        gameManager = FindObjectOfType<GameManager>();

        if (mano2 != null)
        {
            posicionInicialMano = mano2.transform.localPosition;
        }
        else
        {
            Debug.LogWarning("No asignaste Mano2 al jugador.");
        }
    }

    private void Update()
    {
        if (jugadorMuerto)return;
        AtacarEnemigo();
    }

    private void AtacarEnemigo()
    {
        if (mano2 == null) return;
        if (Input.GetMouseButtonDown(1) && !atacando)
        {
            atacando = true;
            golpeAplicado = false;
            tiempoActual = 0f;
            Debug.Log("Ataque activado.");
        }

        if (!atacando) return;
        tiempoActual += Time.deltaTime;
        float mitadAtaque = tiempoAtaque / 2f;
        if (tiempoActual < mitadAtaque)
        {
            float avance = tiempoActual / mitadAtaque;
            mano2.transform.localPosition = Vector3.Lerp(posicionInicialMano,posicionInicialMano + Vector3.forward * distanciaAtaque, avance);
        }
        if (tiempoActual >= mitadAtaque && !golpeAplicado)
        {
            GolpearEnemigo();
            golpeAplicado = true;
        }
        if (tiempoActual >= mitadAtaque)
        {
            float regreso = Mathf.Clamp01((tiempoActual - mitadAtaque) / mitadAtaque);
            mano2.transform.localPosition = Vector3.Lerp(posicionInicialMano + Vector3.forward * distanciaAtaque,posicionInicialMano,regreso);
        }
        if (tiempoActual >= tiempoAtaque)
        {
            mano2.transform.localPosition = posicionInicialMano;
            atacando = false;
            Debug.Log("Ataque terminado.");
        }
    }

    private void GolpearEnemigo()
    {
        Collider[] objetos = Physics.OverlapSphere(mano2.transform.position, radioGolpe,capaEnemigo);

        foreach (Collider objeto in objetos)
        {
            Enemigo enemigo = objeto.GetComponentInParent<Enemigo>();
            if (enemigo != null)
            {
                enemigo.RecibirDaño(dañoAtaque);
                Debug.Log("Golpeaste a: " + enemigo.name);
            }
        }
    }
    public void RecibirDaño(int cantidad)
    {
        if (jugadorMuerto || cantidad <= 0) return;
        if (gameManager != null)
        {
            gameManager.RecibirDano(cantidad);
            if (gameManager.VidaActual <= 0)
            {
                Morir();
            }
        }
        else
        {
            vidaJugador -= cantidad;
            vidaJugador = Mathf.Max(0, vidaJugador);

            Debug.Log("Vida del jugador: " + vidaJugador);

            if (vidaJugador <= 0)
            {
                Morir();
            }
        }
    }
    public void RecibirDano(int cantidad)
    {
        RecibirDaño(cantidad);
    }
    public void SumarPuntaje(int cantidad)
    {
        if (jugadorMuerto || cantidad <= 0) return;         
        if (gameManager != null)
        {
            gameManager.SumarPuntos(cantidad);
        }
        else
        {
           almacenarPuntaje += cantidad;
           Debug.Log("Puntaje actual: " + almacenarPuntaje);
        }
    }
    private void Morir()
    {
        jugadorMuerto = true;
        Debug.Log("GAME OVER");
        Rigidbody rb = GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.linearVelocity = Vector3.zero;
            rb.isKinematic = true;
        }
        Renderer[] renderizadores = GetComponentsInChildren<Renderer>(true);
        foreach (Renderer r in renderizadores)
        {
            r.enabled = false;
        }
        Collider[] colisionadores = GetComponentsInChildren<Collider>(true);
        foreach (Collider c in colisionadores)
        {
            c.enabled = false;
        }
    }
    private void OnDrawGizmosSelected()
    {
        if (mano2 == null) return;
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(mano2.transform.position,radioGolpe);
    }
}
