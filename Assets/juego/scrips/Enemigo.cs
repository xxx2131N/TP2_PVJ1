using UnityEngine;

public class Enemigo : MonoBehaviour
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
    public int Puntaje=100;
    private bool jugadorDetectado= false;
    private float tiempoEntreAtaques=1;
    private void Update()
    {
        if(muerto || ch == null) return;
        temporizadorAtake += Time.deltaTime;
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

        Vector3 destino = ch.transform.position + Vector3.up;
        Vector3 direccion = destino - origen;

        float distancia = direccion.magnitude;

        if (distancia > distanciaVicion) return false;

        float angulo = Vector3.Angle(transform.forward, direccion);

        if (angulo > anguloVicion / 2f)return false;

        if (Physics.Raycast(origen,direccion.normalized,distancia,Obstaculos)) return false;
        
        return true;
    }
    private void atacar()
    {
        if (temporizadorAtake< tiempoEntreAtaques) return;
        ch.RecibirDaño(daño);
        temporizadorAtake =0F;
        Debug.Log("el enemigo atako alPlayer");
    }
    private void MoverHaciaJugador()
    {
        Vector3 direccion= ch.transform.position - transform.position;
        direccion.y = 0f;
        if(direccion.sqrMagnitude> 0.01)
        {
            direccion.Normalize();
            transform.rotation= Quaternion.LookRotation(direccion);
        }
    }
    public void RecibirDaño(float cantidad)
    {
        if(muerto) return;
        vida-=cantidad;
        if(vida <=0) Destruir();
    }
    public void Destruir()
    {
        if(muerto) return;
        muerto=true;
        if(ch != null)
        {
            ch.SumarPuntaje(Puntaje);
        }
        Destroy(gameObject);
    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position,distanciaVicion);

        Vector3 izquierda = Quaternion.Euler(0f, -anguloVicion / 2f, 0f) * transform.forward;

        Vector3 derecha = Quaternion.Euler(0f, anguloVicion / 2f, 0f) * transform.forward;

        Gizmos.color = Color.red;   

        Gizmos.DrawRay(transform.position + Vector3.up * 1.2f,izquierda * distanciaVicion);

        Gizmos.DrawRay(transform.position + Vector3.up * 1.2f,derecha * distanciaVicion);
    }

}
