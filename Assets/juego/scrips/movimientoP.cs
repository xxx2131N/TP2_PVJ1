using UnityEngine;

public class movimientoP : MonoBehaviour
{
    [SerializeField] private float velocidad = 5f;
    [SerializeField] private float fuerzaSalto = 5f;
    private Vector3 direccion = Vector3.zero;
    private Rigidbody rb;

    
    [SerializeField] private LayerMask capaSuelo;
    [SerializeField] private float distanciaSuelo=1f;
    private bool estaEnSuelo = false;

    [SerializeField] private LayerMask capaInteractuable;
    [SerializeField] private float distanciaInteractuable = 3f;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        RaycastHit hitSuelo;
        Debug.DrawRay(transform.position, Vector3.down * distanciaSuelo, Color.red);

        estaEnSuelo = Physics.Raycast(transform.position, Vector3.down, distanciaSuelo, capaSuelo);
        if (estaEnSuelo)
        {
            estaEnSuelo = true;
            Debug.Log("El jugador está tocando el suelo");
        }
        else
        {
            estaEnSuelo = false;
            Debug.Log("El jugador NO está tocando el suelo");
        }
        float inputX = Input.GetAxis("Horizontal");
        float inputY = Input.GetAxis("Vertical");

        direccion = new Vector3(inputX, 0, inputY);

        float velocidadadActual = velocidad;
        if (Input.GetKey(KeyCode.LeftShift))
        {
            velocidadadActual *= 2f;
        }

        rb.linearVelocity =new Vector3(direccion.x * velocidadadActual, rb.linearVelocity.y, direccion.z * velocidadadActual);

        if (Input.GetKeyDown(KeyCode.Space) && estaEnSuelo)
        {
            rb.AddForce(Vector3.up * fuerzaSalto, ForceMode.Impulse);
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            RaycastHit hitInteractuable;
            if (Physics.Raycast(transform.position, transform.forward, out hitInteractuable, distanciaInteractuable, capaInteractuable))
            {
                Debug.Log("El jugador ha interactuado con: " + hitInteractuable.collider.gameObject.name);
            } 
        }
    }
}
