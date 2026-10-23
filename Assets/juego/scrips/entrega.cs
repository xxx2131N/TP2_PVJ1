using UnityEngine;

public class entrega : MonoBehaviour
{
    [SerializeField] private Transform puntoColocacion1;
    [SerializeField] private Transform puntoColocacion2;
    [SerializeField] private Transform puntoColocacion3;

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
        Transform targetActual = transform;

        
        if (esferasEntregadas == 0 && puntoColocacion1 != null) targetActual = puntoColocacion1;
        else if (esferasEntregadas == 1 && puntoColocacion2 != null) targetActual = puntoColocacion2;
        else if (esferasEntregadas == 2 && puntoColocacion3 != null) targetActual = puntoColocacion3;

        item.transform.SetParent(targetActual);
        item.transform.localPosition = Vector3.zero;
        item.transform.localRotation = Quaternion.identity;

        Collider collider = item.GetComponent<Collider>();
        Rigidbody rb = item.GetComponent<Rigidbody>();

        if (rb != null) rb.isKinematic = true;
        if (collider != null) collider.enabled = false;

        Debug.Log("Item entregado en la posición " + (esferasEntregadas + 1));
        
        esferasEntregadas++;

        
        if (esferasEntregadas >= 3)
        {
            Debug.Log("¡Las 3 esferas colocadas! Abriendo puerta...");
        }
    }
}
