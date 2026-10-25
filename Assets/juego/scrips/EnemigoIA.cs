
using UnityEngine;
using UnityEngine.AI;

public class EnemigoIA : MonoBehaviour
{
    [Header("Referencias")]
    [SerializeField] private Transform jugador;
    [SerializeField] private Transform ojos;
    [SerializeField] private Transform[] puntosPatrulla;

    [Header("Vision")]
    [SerializeField] private float rangoVision = 8f;
    [SerializeField, Range(0f, 360f)]
    private float anguloVision = 90f;

    [Header("Patrulla")]
    [SerializeField] private float distanciaLlegada = 1f;

    private NavMeshAgent agente;
    private characterPlayer controladorJugador;

    private int indicePunto = 0;
    private bool persiguiendo = false;

    private void Start()
    {
        agente = GetComponent<NavMeshAgent>();

        if (agente == null)
        {
            Debug.LogError("Falta NavMeshAgent en el enemigo.");
            enabled = false;
            return;
        }

        if (jugador == null)
        {
            GameObject objetoJugador =
                GameObject.FindGameObjectWithTag("Player");

            if (objetoJugador != null)
                jugador = objetoJugador.transform;
        }

        if (jugador == null)
        {
            Debug.LogError("No se encontró al jugador.");
            enabled = false;
            return;
        }

        controladorJugador =
            jugador.GetComponent<characterPlayer>();

        if (ojos == null)
            ojos = transform;

        agente.stoppingDistance = distanciaLlegada;

        if (puntosPatrulla != null &&
            puntosPatrulla.Length > 0)
        {
            IrAlSiguientePunto();
        }
    }

    private void Update()
    {
        if (jugador == null || agente == null)
            return;

        bool detectado = PuedeVerJugador();

        if (detectado)
        {
            persiguiendo = true;

            if (agente.isOnNavMesh)
            {
                agente.isStopped = false;
                agente.SetDestination(jugador.position);
            }
        }
        else
        {
            if (persiguiendo)
            {
                persiguiendo = false;
                IrAlSiguientePunto();
            }

            Patrullar();
        }
    }

    private bool PuedeVerJugador()
    {
        if (controladorJugador != null && !controladorJugador.EstaVisible)
        {
            return false;
        }

        Vector3 origen = ojos.position + Vector3.up * 0.2f;
        Vector3 destino = jugador.position + Vector3.up * 1f;

        Vector3 direccion = destino - origen;
        if (direccion.magnitude > rangoVision)
            return false;

        float angulo = Vector3.Angle(
            ojos.forward,
            direccion
        );

        if (angulo > anguloVision / 2f)
            return false;
        RaycastHit hit;

        if (Physics.Raycast(origen,direccion.normalized,out hit,rangoVision,Physics.DefaultRaycastLayers,QueryTriggerInteraction.Ignore))
        {
            if (hit.transform == jugador ||
                hit.transform.IsChildOf(jugador))
            {
                return true;
            }

            return false;
        }

        return false;
    }

    private void Patrullar()
    {
        if (puntosPatrulla == null ||
            puntosPatrulla.Length == 0)
            return;

        if (!agente.isOnNavMesh)
            return;

        if (!agente.pathPending &&
            agente.remainingDistance <= distanciaLlegada)
        {
            indicePunto =
                (indicePunto + 1) % puntosPatrulla.Length;

            IrAlSiguientePunto();
        }
    }

    private void IrAlSiguientePunto()
    {
        if (puntosPatrulla == null ||
            puntosPatrulla.Length == 0)
            return;

        if (puntosPatrulla[indicePunto] == null)
            return;

        if (agente != null && agente.isOnNavMesh)
        {
            agente.isStopped = false;

            agente.SetDestination(
                puntosPatrulla[indicePunto].position
            );
        }
    }

    private void OnDrawGizmosSelected()
    {
        Transform origen = ojos != null ? ojos : transform;

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(
            origen.position,
            rangoVision
        );
        Vector3 izquierda = Quaternion.Euler(
            0f, -anguloVision / 2f, 0f
        ) * origen.forward;

        Vector3 derecha = Quaternion.Euler(
            0f, anguloVision / 2f, 0f
        ) * origen.forward;

        Gizmos.DrawRay(
            origen.position,
            izquierda * rangoVision
        );

        Gizmos.DrawRay(
            origen.position,
            derecha * rangoVision
        );
    }
}

