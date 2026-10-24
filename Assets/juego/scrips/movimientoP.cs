using UnityEngine;

public class movimientoP : MonoBehaviour
{
    [SerializeField] private float velocidad = 5f;
    [SerializeField] private float fuerzaSalto = 5f;
    private Vector3 direccion = Vector3.zero;
    private Rigidbody rb;

    [Header("Camara")]
    [SerializeField] private Transform camara;
    [SerializeField] private float sensibilidad =100f; 

    private float rotacionX = 0;
    
    [SerializeField] private LayerMask capaSuelo;
    [SerializeField] private float distanciaSuelo=1.1f;
    private bool estaEnSuelo = false;

    [SerializeField] private LayerMask capaInteractuable;
    [SerializeField] private float distanciaInteractuable = 3f;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody>();

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    // Update is called once per frame
    void Update()
    {
        float mouseX= Input.GetAxis("Mouse X") * sensibilidad *Time.deltaTime;
        float mouseY= Input.GetAxis("Mouse Y") * sensibilidad *Time.deltaTime;
        transform.Rotate(0f,mouseX,0f);
        rotacionX -= mouseY;
        rotacionX = Mathf.Clamp(rotacionX, -90,90);
        if(camara != null)
        {
            camara.localRotation = Quaternion.Euler(rotacionX,0,0);
        }
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

        direccion = transform.right* inputX + transform.forward* inputY;
        direccion.Normalize();

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
    private void Interactuar()
{
    if (camara == null)
    {
        Debug.LogWarning("No asignaste la camara");
        return;
    }
    RaycastHit hit;
    Debug.DrawRay(
        camara.position,
        camara.forward * distanciaInteractuable,
        Color.red
    );
    if (Physics.Raycast(camara.position,camara.forward,out hit,distanciaInteractuable,capaInteractuable))
    {
        Debug.Log("Objeto detectado: " + hit.collider.gameObject.name);

        if (Input.GetKeyDown(KeyCode.E))
        {
            Boton boton = hit.collider.GetComponent<Boton>();

            if (boton != null)
            {
                boton.Activar();
            }
            else
            {
                Debug.Log("Detectaste el objeto, pero todavía no tiene una acción asignada");
            }
        }
    }
}
}
