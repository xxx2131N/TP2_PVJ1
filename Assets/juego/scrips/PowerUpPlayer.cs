using UnityEngine;
using System.Collections;

public class PowerUpPlayer : MonoBehaviour
{
    [Header("Poder invisible")]
    [SerializeField] private float duracionPoder=3f;
    [SerializeField] private float tiempoRecarga=5f;
    [Header ("cuerpo y mano")]
    [SerializeField] private GameObject mano1;
    [SerializeField] private GameObject mano2;
    [SerializeField] private GameObject cuerpoP;
    private bool visible = true;
    public bool EstaVisible=> visible;
    private bool poderDisponible= true;
    private bool jugadorMuerto= false;
    private Renderer[] renderizadoresJugador;
    private Renderer[] renderizadoresMano1;
    private Renderer[] renderizadoresMano2;
    private void Start()
    {
        renderizadoresJugador = GetComponentsInChildren<Renderer>(true);
        if(mano1 != null) renderizadoresMano1= mano1.GetComponentsInChildren<Renderer>(true);
        if(mano2 != null) renderizadoresMano2= mano2.GetComponentsInChildren<Renderer>(true);
    }
    private void Update()
    {
        activarPoder();
    }
    private void activarPoder()
    {
        if (Input.GetKeyDown(KeyCode.Q) && poderDisponible && !jugadorMuerto)
        {
            StartCoroutine(PoderInvisibilidad());
            Debug.Log("PowerUp activado");
        }
    }
    public IEnumerator PoderInvisibilidad()
    {
        poderDisponible = false;
        visible = false;
        CambiarVisibilidad(false);

        Debug.Log("Invisibilidad activada");
        yield return new WaitForSeconds(duracionPoder);
        if (!jugadorMuerto)
        {
            visible = true;
            CambiarVisibilidad(true);

            Debug.Log("Invisibilidad desactivada");
        }
        yield return new WaitForSeconds(tiempoRecarga);

        if (!jugadorMuerto)
        {
            poderDisponible = true;
            Debug.Log("Poder disponible nuevamente");
        }
    }

    private void CambiarVisibilidad(bool estado)
    {
        CambiarRenderizadores(renderizadoresJugador,estado);
        CambiarRenderizadores(renderizadoresMano1,estado);
        CambiarRenderizadores(renderizadoresMano2,estado);
    }
    private void CambiarRenderizadores(Renderer[] renderizadores, bool estado)
    {
        if (renderizadores == null) return;

        foreach (Renderer r in renderizadores)
        {
            if (r != null)r.enabled = estado;
        }
    }

    public void MarcarJugadorMuerto()
    {
        jugadorMuerto = true;
        StopAllCoroutines();
        visible = false;
        CambiarVisibilidad(false);
    }
}
