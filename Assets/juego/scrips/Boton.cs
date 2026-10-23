using UnityEngine;

public class Boton : MonoBehaviour
{
    [SerializeField] private color colorV;
    [SerializeField] private color colorR;
    [SerializeField] private bool botonPresionado = false;


    private void cambioColor()
    {
        if (botonPresionado)
        {
            GetComponent<Renderer>().material.color = colorV;
        }
        else
        {
            GetComponent<Renderer>().material.color = colorR;
        }
    }

    private void importarBotonAccionar()
    {
        [SerializeField] boton1;
        [SerializeField] boton2;
        [SerializeField] boton3;
    }


}
