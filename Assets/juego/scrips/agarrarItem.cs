using UnityEngine;

public class agarrarItem : MonoBehaviour
{
    [SerializeField] private Transform mano1;
    private GameObject currentItem=null;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Item") && currentItem == null)
        {
            if(Input.GetKeyDown(KeyCode.E)) Pick(other.gameObject);
        }
    }

    private void Pick(GameObject item)
    {
        currentItem = item;     
        item.transform.SetParent(mano1);
        item.transform.localPosition = Vector3.zero;
        item.transform.localRotation = Quaternion.identity;

        Rigidbody rb = item.GetComponent<Rigidbody>();
        Collider collider = item.GetComponent<Collider>();
        if (rb != null) rb.isKinematic = true;
        if (collider != null) collider.enabled = false;
    }

    public GameObject DropItem()
    {
        GameObject temp = currentItem;
        currentItem = null;
        return temp;
    }


}
