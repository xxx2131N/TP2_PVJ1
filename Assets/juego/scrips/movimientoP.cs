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
    [SerializeField] private InterfacePlayer interfacePlayer;

    private float rotacionX = 0;
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        if (interfacePlayer == null) interfacePlayer = GetComponent<InterfacePlayer>();

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
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

        if (Input.GetKeyDown(KeyCode.Space) && interfacePlayer != null && interfacePlayer.EstaEnSuelo)
        {
            rb.AddForce(Vector3.up * fuerzaSalto, ForceMode.Impulse);
        }
    }
}

