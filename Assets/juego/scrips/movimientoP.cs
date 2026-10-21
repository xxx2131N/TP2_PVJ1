using UnityEngine;

public class movimientoP : MonoBehaviour
{
    [SerializeField] private float velocidad = 5f;
    private Vector3 direccion = Vector3.zero;
    private Rigidbody rb;
    private bool estaEnSuelo = false;
    [SerializeField] private LayerMask capaSuelo;
    [SerializeField] private float distanciaSuelo=0.2f;
    [SerializeField] private float fuerzaSalto = 5f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        RaycastHit hit;
        if(Physics.Raycast(transform.position, Vector3.down, out hit, distanciaSuelo, capaSuelo))
        {
            Debug.Log("El jugador está tocando el suelo");
        }
        float inputX = Input.GetAxis("Horizontal");
        float inputY = Input.GetAxis("Vertical");

        direccion = new Vector3(inputX, 0, inputY);

        if (Input.GetKey(KeyCode.LeftShift))
        {
            rb.linearVelocity = direccion * velocidad * 2f;
        }
        else
        {
            rb.linearVelocity = direccion * velocidad;
        }
        if (Input.GetKeyDown("Jump") && distanciaSuelo)
        {
            rb.AddForce(Vector3.up * fuerzaSalto, ForceMode.Impulse);
        }
    }
}
