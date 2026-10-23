using UnityEngine;

public class agarrarItem : MonoBehaviour
{
    [SerializeField] private Transform mano1;

    private GameObject currentItem=null;
    private GameObject itemCercano = null;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (currentItem == null && itemCercano != null)
            {
                Pick(itemCercano);
            }
        }
    }
    private void Pick(GameObject item)
    {
        currentItem = item;     
        itemCercano = null;

        item.transform.SetParent(mano1);
        item.transform.localPosition = Vector3.zero;
        item.transform.localRotation = Quaternion.identity;

        Rigidbody rb = item.GetComponent<Rigidbody>();
        Collider collider = item.GetComponent<Collider>();

        if (rb != null) rb.isKinematic = true;
        if (collider != null) collider.enabled = false;
        Debug.Log("Item agarrado ");
    }

    public GameObject DropItem()
    {
        GameObject temp = currentItem;
        currentItem = null;
        Debug.Log("Item soltado ");

        return temp;
    }

}
