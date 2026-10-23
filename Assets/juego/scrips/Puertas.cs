using UnityEngine;

public class Puertas : MonoBehaviour
{
    [SerializeField] private Transform puerta1;
    [SerializeField] private Transform puerta2;
    [SerializeField] private Transform puerta3;

    private int esferasEntregadas = 0;
    private int botonAccionado=0; 

    private void AbrirPuerta1()
    {
        if (botonAccionado >= 3)
        {
            AbrirPuerta1();
            Debug.Log("¡Puerta 1 abierta!");
            puerta1.gameObject.SetActive(false);
        }
        else
        {
            Debug.Log("Aún no se ha presionado el botón.");
        }
    }

    public void AbrirPuerta2()
    {
        if (esferasEntregadas >= 3)
        {
            AbrirPuerta2();
            Debug.Log("¡Puerta 2 abierta!");
            puerta2.gameObject.SetActive(false);
        }
        else
        {
            Debug.Log("Aún no se han entregado las 3 esferas.");
        }
    }
    private void AbrirPuerta3()
    {
        if (esferasEntregadas >= 3)
        {
            AbrirPuerta3();
            Debug.Log("¡Puerta 3 abierta!");
            puerta3.gameObject.SetActive(false);
        }
        else
        {
            Debug.Log("Aún no se han entregado las 3 esferas.");
        }
    }
}
