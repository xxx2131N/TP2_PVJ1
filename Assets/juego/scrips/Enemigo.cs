using UnityEngine;

public class VidaEnemiga : MonoBehaviour
{
    [SerializeField] private GameObject manoD1;
    [SerializeField] private GameObject manoI2;
    [SerializeField] public GameObject mano2;
    [SerializeField] private float vida = 6f;
    [SerializeField] private characterPlayer ch;
    private float distanciaVicion= 5f;
    [SerializeField, Range(0f,360f)] private float anguloVicion =90f;
    private LayerMask Obstaculos;
    private int daño=2;
    private float velocidad=5;
    private float distanciaAtake=1.5f;
    private float velocidadAtake=3;
    private float temporizadorAtake;
    private bool muerto=false;
    private void Update()
    {
        if(muerto || ch == null) return;
        temporizadorAtake += temporizadorAtake.deltaTime;
        jugadorDetectado= DetectarJugador();
        if(!jugadorDetectado) return;
        float distancia= Vector3.Distance(transform.position,ch.transform.position);
        if (distancia> distanciaAtake)
        {
            MoverHaciaJugador();
        }
        else
        {
            atacar();
        }
    }
    private bool DetectarJugador()
    {
        Vector3 origen = transform.position + Vector3.up * 1.2f;

        Vector3 destino = jugador.transform.position + Vector3.up;
        Vector3 direccion = destino - origen;

        float distancia = direccion.magnitude;

        if (distancia > distanciaVision) return false;

        float angulo = Vector3.Angle(transform.forward, direccion);

        if (angulo > anguloVision / 2f)return false;

        if (Physics.Raycast(origen,direccion.normalized,distancia,obstaculos)) return false;
        
        return true;
    }

    private void MoverHaciaJugador()
    {
        Vector3 direccion = jugador.transform.position - transform.position;

        direccion.y = 0f;

        if (direccion.sqrMagnitude > 0.01f)
        {
            direccion.Normalize();

            transform.position += direccion * velocidad * Time.deltaTime;
            transform.rotation = Quaternion.LookRotation(direccion);
        }
    }


    private void atacar()
    {
        if (temporizadorAtake< temporizadorAtake) return;
        ch.RecibirDaño(daño);
        temporizadorAtake =0F
        Debug.Log("el enemigo atako alPlayer");
    }
    private void MoverHaciaJugador()
    {
        Vector3direccion= ch.transform.position - transform.position;
        direccion.y=0f;
        if(direccion.sqrMagnitude> 0.01)
        {
            direccion.Normalize();
            transform.rotation= Quaternion.LookRotation(direccion);
        }
    }
    private void RecibirDaño(float cantidad)
    {
        if(muerto) return;
        vida-=cantidad;
        if(vida <=0) Destruir();
    }
    private void Destruir()
    {
        muerto= true;
        Destroy(GameObject);
    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position,distanciaVision);

        Vector3 izquierda = Quaternion.Euler(0f, -anguloVision / 2f, 0f) * transform.forward;

        Vector3 derecha = Quaternion.Euler(0f, anguloVision / 2f, 0f) * transform.forward;

        Gizmos.color = Color.red;   

        Gizmos.DrawRay(transform.position + Vector3.up * 1.2f,izquierda * distanciaVision);

        Gizmos.DrawRay(transform.position + Vector3.up * 1.2f,derecha * distanciaVision);
    }

}
