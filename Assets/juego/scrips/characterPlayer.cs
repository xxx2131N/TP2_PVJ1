using UnityEngine;
using System.Collections;

public class characterPlayer : MonoBehaviour
{
    private Rigidbody rb;
    private GameManager gameManager;

    private bool jugadorMuerto = false;


    [Header("Interaccion")]
    [SerializeField] private Transform camara;
    [SerializeField] private LayerMask capaInteractuable;
    [SerializeField] private float distanciaInteractuable = 3f;

    [Header("Poder")]
    [SerializeField] private float duracionPoder=3f;
    [SerializeField] private float tiempoRecarga=5f;

    private bool visible = true;
    public bool EstaVisible=> visible;
    private bool poderDisponible= true;
    private Renderer[] renderizadoresJugador;

    [Header("Ataque")]
    [SerializeField] private GameObject mano2;
    [SerializeField] private float dañoAtaque = 2f;
    [SerializeField] private float distanciaAtaque = 2f;
    [SerializeField] private float tiempoAtaque = 1f;
    [SerializeField] private float radioGolpe = 0.5f;

    [Header("Daño recibido")]
    [SerializeField] private int dañoRecibido = 2;

    private Vector3 posicionInicialMano;

    private bool atacando = false;
    private bool golpeAplicado = false;

    private float tiempoActual = 0f;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        renderizadoresJugador  = GetComponentsInChildren<Renderer>(true);
        gameManager= FindObjectOfType<GameManager>();

        if (mano2 != null)
        {
            posicionInicialMano = mano2.transform.localPosition;
        }
        if (gameManager == null)
        {
            Debug.LogError("No se encontró el GameManager en la escena.");
        }
        
    }

    void Update()
    {
        if (jugadorMuerto)
        {
            return;
        }
        Interactuar();

        activarPoder();
        atacarEnemigo();
    }

    private void activarPoder()
    {
        if (Input.GetKeyDown(KeyCode.Q) && poderDisponible)
        {
            StartCoroutine(PoderInvisibilidad());
            Debug.Log("PowerUp activado");
        }
    }
    public IEnumerator PoderInvisibilidad()
    {
        poderDisponible = false;
        visible = false;
        CambiarVisibilidad(false);

        Debug.Log("Invisibilidad activada");
        yield return new WaitForSeconds(duracionPoder);
        if (!jugadorMuerto)
        {
            visible = true;
            CambiarVisibilidad(true);

            Debug.Log("Invisibilidad desactivada");
        }
        yield return new WaitForSeconds(tiempoRecarga);

        if (!jugadorMuerto)
        {
            poderDisponible = true;
            Debug.Log("Poder disponible nuevamente");
        }
    }

    private void CambiarVisibilidad(bool estado)
    {
        foreach (Renderer r in renderizadoresJugador)
        {
            if (r != null)
            {
                r.enabled = estado;
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (!collision.gameObject.CompareTag("Enemigo"))
        return;
        if (gameManager != null)
        {
            gameManager.RecibirDano(dañoRecibido);
        }
    }

    private void atacarEnemigo()
    {
        if (Input.GetMouseButtonDown(1) && !atacando)
        {
            if (visible)
            {
                atacando = true;
                golpeAplicado = false;
                tiempoActual = 0f;

                Debug.Log("Ataque activado");
            }
        }

        if (atacando)
        {
            tiempoActual += Time.deltaTime;

            if (tiempoActual < tiempoAtaque / 2f)
            {
                float avance = tiempoActual / (tiempoAtaque / 2f);

                mano2.transform.localPosition =
                    Vector3.Lerp(
                        posicionInicialMano,
                        posicionInicialMano + Vector3.forward * distanciaAtaque,
                        avance
                    );
            }

            if (tiempoActual >= tiempoAtaque / 2f && !golpeAplicado)
            {
                GolpearEnemigo();
                golpeAplicado = true;
            }

            if (tiempoActual >= tiempoAtaque / 2f)
            {
                float regreso =
                    (tiempoActual - tiempoAtaque / 2f)
                    / (tiempoAtaque / 2f);

                mano2.transform.localPosition =
                    Vector3.Lerp(
                        posicionInicialMano + Vector3.forward * distanciaAtaque,
                        posicionInicialMano,
                        regreso
                    );
            }

            if (tiempoActual >= tiempoAtaque)
            {
                mano2.transform.localPosition = posicionInicialMano;

                atacando = false;

                Debug.Log("Ataque terminado. Puedes volver a atacar.");
            }
        }
    }
    private void Interactuar()
    {
        if (camara == null)
        {
            return;
        }
        float distancia=2f;
        Vector3 origuen = camara.position;
        Vector3 direccionRayo= camara.forward;
        RaycastHit hit;
        Debug.DrawRay(origuen,direccionRayo*distancia,Color.red);
        if (Physics.Raycast(camara.position,camara.forward,out hit,distanciaInteractuable,capaInteractuable))
        {
            Debug.Log("Objeto detectado: " + hit.collider.gameObject.name);

            if (Input.GetKeyDown(KeyCode.E))
            {
                Boton boton = hit.collider.GetComponent<Boton>();
                if(boton == null) boton= hit.collider.GetComponentInParent<Boton>();

                if (boton != null)
                {
                    boton.Activar();
                }
            }
        }
    }

    private void GolpearEnemigo()
    {
        Collider[] objetos = Physics.OverlapSphere(
            mano2.transform.position,
            radioGolpe,
            capaEnemigo
        );

        foreach (Collider objeto in objetos)
        {
            objeto.SendMessage(
                "RecibirDanio",
                dañoAtaque,
                SendMessageOptions.DontRequireReceiver
            );

            Debug.Log("¡Golpeaste a: " + objeto.gameObject.name);
        }
    }

    private void Morir()
    {
        jugadorMuerto = true;

        Debug.Log("GAME OVER");

        if (rb != null)
        {
            rb.linearVelocity = Vector3.zero;
            rb.isKinematic = true;
        }

        Renderer[] renderizadores =
            GetComponentsInChildren<Renderer>();

        foreach (Renderer r in renderizadores)
        {
            r.enabled = false;
        }

        Collider[] colisionadores =
            GetComponentsInChildren<Collider>();

        foreach (Collider c in colisionadores)
        {
            c.enabled = false;
        }
    }
}

