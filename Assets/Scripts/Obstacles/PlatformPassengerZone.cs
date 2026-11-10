using UnityEngine;

public class PlatformPassengerZone : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player"))
            return;

        if (other.GetComponent<CharacterController>() == null)
            return;

        other.transform.SetParent(transform.parent, true);
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Player"))
            return;

        if (other.transform.parent == transform.parent)
        {
            other.transform.SetParent(null, true);
            other.transform.SetParent(null, true);
        }
    }
}
