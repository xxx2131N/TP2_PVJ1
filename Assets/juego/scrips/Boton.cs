using UnityEngine;

public class Boton : MonoBehaviour
{
    [SerializeField] private Color colorV;
    [SerializeField] private Color colorR;
    [SerializeField] private bool botonPresionado = false;
    [SerializeField] private Puertas controladorPuertas;

    private void Start()
    {
        cambioColor();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !botonPresionado)
        {
            botonPresionado = true;
            cambioColor();
            if (controladorPuertas != null)
            {
                controladorPuertas.RegistrarBotonPresionado();
            }
        }
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
