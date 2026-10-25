using UnityEngine;

public class entrega : MonoBehaviour
{
    [Header("Punto de entrega")]
    [SerializeField] private Transform puntoColocacion;

    [Header("puerta")]
    [SerializeField] private GameObject Puerta;
    [SerializeField] private float distanciaApertura=3;
    private bool esferaEntregada= false;
    private Vector3 posicionInicialPuerta;
    private void Start()
    {
        if (Puerta!=null)posicionInicialPuerta = Puerta.transform.position;
    }
    public bool EntregarEsfera(GameObject objetoEsfera)
    {
        if (esferaEntregada)
        {
            Debug.Log("La esfera ya fue entregada.");
            return false;
        }

        if (objetoEsfera == null || puntoColocacion == null)
        {
            Debug.LogWarning("Asigná el punto de colocación y comprobá la esfera si esta.");
            return false;
        }
        objetoEsfera.transform.SetParent(puntoColocacion);
        objetoEsfera.transform.localPosition = Vector3.zero;
        objetoEsfera.transform.localRotation = Quaternion.identity;
        Rigidbody rb = objetoEsfera.GetComponent<Rigidbody>();

        if (rb != null)
        {
            rb.isKinematic = true;
            rb.linearVelocity = Vector3.zero;
        }

        Collider col = objetoEsfera.GetComponent<Collider>();

        if (col != null) col.enabled = false;
        esferaEntregada = true;
        AbrirPuerta();
        Debug.Log("¡Esfera entregada!");
        return true;
    }
    private void AbrirPuerta()
    {
        if (Puerta == null)
        {
            Debug.LogWarning("No asignaste la puerta de entrega.");
            return;
        }
        Puerta.transform.position = posicionInicialPuerta + Vector3.down * distanciaApertura;
        Debug.Log("¡Puerta de entrega abierta!");
    }
}