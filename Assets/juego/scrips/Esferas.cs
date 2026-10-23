using UnityEngine;

public class Esferas : MonoBehaviour
{
    [SerializeField] private Transform puntoEntrega;
    private int esferasEntregadas = 0;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            AgarrarItem playerAgarrar = other.GetComponent<AgarrarItem>();
            if (playerAgarrar != null)
            {
                GameObject item = playerAgarrar.DropItem();
                if (item != null)
                {
                    ItemEntregado(item);
                }
            }
        }
    }

    private void cantidadEsferas()
    {
        [SerializeField] esfera1;
        [SerializeField] esfera2;
        [SerializeField] esfera3;
        
    }


}
