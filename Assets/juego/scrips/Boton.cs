using UnityEngine;

public class Boton : MonoBehaviour
{
    [SerializeField] private Color colorV;
    [SerializeField] private Color colorR;
    [SerializeField] private bool botonPresionado = false;
    [SerializeField] private Puertas controladorPuertas;
    private Renderer objetoRenderer;

    private void Start()
    {
        objetoRenderer = GetComponent<Renderer>();
        cambioColor();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !botonPresionado)
        {
            Activar();
        }
    }
    public void Activar()
    {
        if (botonPresionado)
        {
            return;
        }
        botonPresionado= true;
        cambioColor();
        if(controladorPuertas != null)
        {
            controladorPuertas.RegistrarBotonPresionado();
        }
        Debug.Log("activado voton");
    }


    private void cambioColor()
    {
        Renderer renderer = GetComponent<Renderer>();
        if (renderer != null)
        {
            if (botonPresionado)
            {
                renderer.material.color = colorV;
            }
            else
            {
                renderer.material.color = colorR;
            }
        }
        
    }


}
