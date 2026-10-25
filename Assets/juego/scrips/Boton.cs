using UnityEngine;

public class Boton : MonoBehaviour
{
    [Header("colores")]
    [SerializeField] private Color colorActivado = Color.green;
    [SerializeField] private Color colorDesactivado = Color.red;
    [SerializeField] private GameObject Puerta;
    [SerializeField] private float distanciaApertura= 3;
    private Renderer objetoRenderer;
    private bool botonPresionado = false;
    private Vector3 posicionInicialPuerta; 

    private void Start()
    {
        objetoRenderer = GetComponent<Renderer>();
        if (Puerta != null) posicionInicialPuerta=Puerta.transform.position;
        CambiarColor();
    }
    public void Activar()
    {
        if (botonPresionado)
        {
            Debug.Log("El botón ya fue activado.");
            return;
        }

        botonPresionado = true;

        CambiarColor();
        AbrirPuerta();

        Debug.Log("¡Botón activado!");
    }
    private void CambiarColor()
    {
        if (objetoRenderer == null)
            return;

        objetoRenderer.material.color = botonPresionado ? colorActivado : colorDesactivado;
    }

    private void AbrirPuerta()
    {
        if (Puerta== null)
        {
            Debug.LogWarning("No asignaste una puerta al botón.");
            return;
        }

        Puerta.transform.position = posicionInicialPuerta + Vector3.down * distanciaApertura;
        Debug.Log("¡Puerta abierta!");
    }
    


}
