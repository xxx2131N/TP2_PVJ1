using UnityEngine;

public class Spawner : MonoBehaviour
{
    [SerializeFild] private GameObject enemigo;
    private float tiempoCreacion = 4f;
    private int cantidadMaxima=3;
    private int cantidadInvocada=0;
    private float temporizador= 0f;
    private void Update()
    {
        if (cantidadInvocada >= cantidadMaxima)
        return;
        temporizador += Time.deltaTime;
        if(temporizador >= tiempoCreacion)
        {
            crearEnemigo();
            temporizador=0f;
        }
    }
    public void crearEnemigo()
    {
        if (cantidadInvocada>= cantidadMaxima)
        return;
        if(enemigo=null)
        {
            Debug.LogWarning("ubo un errror");
            return;
        }
        Instantiate(enemigo,transform.position,transform.rotation);
        cantidadInvocada++;
        Debug.Log("se creo un enemigo");
    }

}
