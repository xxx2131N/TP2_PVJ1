using UnityEngine;

public class characterPlayer : MonoBehaviour
{
    private Rigidbody rb;

    [Header("Vida")]
    [SerializeField] private int vida = 10;
    [SerializeField] private int dañoRecibido = 2;

    private bool jugadorMuerto = false;

    [Header("Poder")]
    [SerializeField] private GameObject mano2;
    [SerializeField] private GameObject mano1;

    private bool visible = true;

    [Header("Ataque")]
    [SerializeField] private float dañoAtaque = 2f;
    [SerializeField] private float distanciaAtaque = 0.8f;
    [SerializeField] private float tiempoAtaque = 1f;
    [SerializeField] private float radioGolpe = 0.5f;

    [SerializeField] private LayerMask capaEnemigo;

    private Vector3 posicionInicialMano;

    private bool atacando = false;
    private bool golpeAplicado = false;

    private float tiempoActual = 0f;

    void Start()
    {
        rb = GetComponent<Rigidbody>();

        if (mano2 != null)
        {
            posicionInicialMano = mano2.transform.localPosition;
        }
    }

    void Update()
    {
        if (jugadorMuerto)
        {
            return;
        }

        activarPoder();
        atacarEnemigo();
    }

    private void activarPoder()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            visible = false;

            mano2.SetActive(false);
            mano1.SetActive(false);

            Debug.Log("PowerUp activado");
        }

        if (Input.GetKeyUp(KeyCode.Q))
        {
            visible = true;

            mano2.SetActive(true);
            mano1.SetActive(true);

            Debug.Log("PowerUp desactivado");
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Enemigo"))
        {
            vida -= dañoRecibido;

            if (vida < 0)
            {
                vida = 0;
            }

            Debug.Log("Vida del jugador: " + vida);

            if (vida <= 0)
            {
                Morir();
            }
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

    private void OnGUI()
    {
        GUI.Box(new Rect(10, 10, 180, 65), "VIDA DEL JUGADOR");

        GUI.Label(
            new Rect(20, 35, 160, 25),
            "Vida: " + vida + " / 10"
        );

        GUI.Box(
            new Rect(20, 58, 150, 10),
            ""
        );

        GUI.Box(
            new Rect(20, 58, 150f * vida / 10f, 10),
            ""
        );

        if (jugadorMuerto)
        {
            GUI.Box(
                new Rect(Screen.width / 2 - 100,
                         Screen.height / 2 - 30,
                         200, 60),
                "GAME OVER"
            );
        }
    }
}

