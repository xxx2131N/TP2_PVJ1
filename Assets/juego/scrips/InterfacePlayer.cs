using UnityEngine;

public class InterfacePlayer : MonoBehaviour
{
    [Header("Detección del suelo")]
    [SerializeField] private float distanciaSuelo = 1.1f;
    [SerializeField] private LayerMask capaSuelo;
    [SerializeField] private Transform pies;

    [Header("Interacción frontal")]
    [SerializeField] private float distanciaInteraccion = 2f;
    [SerializeField] private Transform camara;
    [SerializeField] private LayerMask capaInteractuable;
    [SerializeField] public GameObject esfera;
    [SerializeField] public GameObject mano1;
    private bool estaEnSuelo;
    private bool llevandoEsfera;

    public bool EstaEnSuelo => estaEnSuelo;

    private void Update()
    {
        DetectarSuelo();
        if (Input.GetKeyDown(KeyCode.E))
        {
            DetectarInteraccion();
        }
    }

    private void DetectarSuelo()
    {
        if (pies == null)return;

        estaEnSuelo = Physics.Raycast(pies.position,Vector3.down,distanciaSuelo,capaSuelo);
        Debug.Log("estaen suelo? " + estaEnSuelo);
    }

    private void DetectarInteraccion()
    {
        if (camara == null) return;

        if (Physics.Raycast(camara.position,camara.forward,out RaycastHit impacto,distanciaInteraccion,capaInteractuable))
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                Debug.Log("Objeto detectado");

                Boton boton = impacto.collider.GetComponent<Boton>();

                if (boton == null)
                    boton = impacto.collider.GetComponentInParent<Boton>();

                if (boton != null)
                {
                    boton.Activar();
                    return;
                }
                entrega puntoEntrega= impacto.collider.GetComponentInParent<entrega>();
                if(puntoEntrega != null)
                {
                    if (llevandoEsfera)
                    {
                        if (puntoEntrega.EntregarEsfera(esfera))
                        {
                            llevandoEsfera=false;
                            esfera= null;
                            Debug.Log("se entrego la esfera");
                        }
                    }
                    else
                    {
                        Debug.Log("dever primero recojer la esfera");
                    }
                    return;
                }
                if (esfera != null && (impacto.collider.gameObject == esfera || impacto.collider.transform.IsChildOf(esfera.transform)))
                {
                    RecogerEsfera();
                    return;
                }

                Debug.Log("El objeto detectado no tiene una interacción configurada.");
                
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        if (pies != null)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawLine(pies.position,pies.position + Vector3.down * distanciaSuelo);
        }

        if (camara != null)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawLine(camara.position,camara.position + camara.forward * distanciaInteraccion);
        }
    }
    private void RecogerEsfera()
    {
        if (llevandoEsfera)
        {
            SoltarEsfera();
            return;
        }

        if (esfera == null || mano1 == null)
        {
            Debug.LogWarning("Asigná la esfera y la mano en el Inspector.");
            return;
        }

        llevandoEsfera = true;
        esfera.transform.SetParent(mano1.transform);
        esfera.transform.localPosition = Vector3.zero;
        esfera.transform.localRotation = Quaternion.identity;
        Rigidbody rb = esfera.GetComponent<Rigidbody>();

        if (rb != null)
        {
            rb.isKinematic = true;
            rb.linearVelocity = Vector3.zero;
        }

        Collider col = esfera.GetComponent<Collider>();

        if (col != null) col.enabled = false;
        Debug.Log("¡Esfera recogida!");
    }

    private void SoltarEsfera()
    {
        if (esfera == null) return;

        llevandoEsfera = false;
        esfera.transform.SetParent(null);
        esfera.transform.position = camara.position + camara.forward * 1f;
        Rigidbody rb = esfera.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.isKinematic = false;
        }

        Collider col = esfera.GetComponent<Collider>();
        if (col != null) col.enabled = true;
        Debug.Log("Esfera soltada.");
    }

}
