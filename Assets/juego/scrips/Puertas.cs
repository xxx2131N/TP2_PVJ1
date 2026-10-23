using UnityEngine;

public class Puertas : MonoBehaviour
{
    [SerializeField] private Transform puerta1;
    [SerializeField] private Transform puerta2;
    [SerializeField] private Transform puerta3;

    private int esferasEntregadas = 0;
    private int botonAccionado=0; 

    public void RegistrarBotonPresionado()
    {
        botonAccionado++;
        Debug.Log("Botón precionado");
        VerificarCondiciones();
    }

    public void RegistrarEsferaEntregada()
    {
        esferasEntregadas++;
        Debug.Log("Esfera entregada");
        VerificarCondiciones();
    }

    public void VerificarCondiciones()
    {
        if(botonAccionado >= 3 && puerta1 != null)
        {
            puerta1.gameObject.SetActive(false);
            Debug.Log("Puerta 1 abierta");
        }
        if(esferasEntregadas >= 3 && puerta2 != null)
        {
            puerta2.gameObject.SetActive(false);
            Debug.Log("Puerta 2 abierta");
        }
        if(esferasEntregadas >= 3 && puerta3 != null)
        {
            puerta3.gameObject.SetActive(false);
            Debug.Log("Puerta 3 abierta");
        }
    }
}
