using UnityEngine;

public class VidaEnemiga : MonoBehaviour
{
    [SerializeField] private float vida = 6f;
    public void RecibirDaño(float cantidad)
    {
        vida -= cantidad;
        if (vida <= 0f)
        {
            Destroy(gameObject);
            Debug.Log("Enemigo destruido");
        }
    }
}
