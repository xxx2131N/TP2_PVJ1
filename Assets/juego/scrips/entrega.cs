using UnityEngine;

public class entrega : MonoBehaviour
{
[Header("Punto de entrega")]
[SerializeField] private Transform puntoColocacion;

[Header("Control de puertas")]
[SerializeField] private Puertas controlPuertas;

private void OnTriggerEnter(Collider other)
{
    if (!other.CompareTag("Player")) return;

    agarrarItem jugador = other.GetComponent<agarrarItem>();
    if (jugador == null) return;

    GameObject esfera = jugador.DropItem();
    if (esfera == null) return;

    esfera.transform.SetParent(puntoColocacion);
    esfera.transform.localPosition = Vector3.zero;
    esfera.transform.localRotation = Quaternion.identity;

    Rigidbody rb = esfera.GetComponent<Rigidbody>();
    if (rb != null)
    {
        rb.isKinematic = true;
    }

    Collider col = esfera.GetComponent<Collider>();
    if (col != null)
    {
        col.enabled = false;
    }

    if (controlPuertas != null)
    {
        controlPuertas.RegistrarEsferaEntregada();
    }

    Debug.Log("¡Esfera entregada!");
}

}