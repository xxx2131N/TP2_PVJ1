using UnityEngine;

public class entrega : MonoBehaviour
{
    [SerializeField] private Transform puntoColocacion1;
    [SerializeField] private Transform puntoColocacion2;
    [SerializeField] private Transform puntoColocacion3;

    [SerializeField] private Puertas controlPuertas;

    private int esferasEntregadas = 0;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            agarrarItem playerAgarrar = other.GetComponent<agarrarItem>();
            if (playerAgarrar != null)
            {
                GameObject item = playerAgarrar.DropItem();
                if (item != null)
                {
                    ItemEntregado(item);
                }
            }
        }
    }

    private void ItemEntregado(GameObject item)
    {
        Transform targetActual = null;

        if (esferasEntregadas == 0) targetActual = puntoColocacion1;
        else if (esferasEntregadas == 1) targetActual = puntoColocacion2;
        else if (esferasEntregadas == 2) targetActual = puntoColocacion3;

        if (targetActual != null)
        {
            item.transform.SetParent(targetActual);
            item.transform.localPosition = Vector3.zero;
            item.transform.localRotation = Quaternion.identity;

            Collider collider = item.GetComponent<Collider>();
            Rigidbody rb = item.GetComponent<Rigidbody>();

            if (rb != null) rb.isKinematic = true;
            if (collider != null) collider.enabled = false;
        
            esferasEntregadas++;

        
            if (controlPuertas != null)
            {
                controlPuertas.RegistrarEsferaEntregada();
            }
            else
            {
                Debug.LogWarning("Controlador de puertas no asignado en el inspector.");
            }
        }
        
    }
}
